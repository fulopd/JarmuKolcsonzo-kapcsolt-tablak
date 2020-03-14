using JarmuKolcsonzo.Models;
using JarmuKolcsonzo.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarmuKolcsonzo.Repositories
{
    public class RendelesRepository : IDisposable
    {
        private JKContext db = new JKContext();
        private int _totalItems;

        public BindingList<rendelesVM> GetAllRendelesVM(
            int page = 0,
            int itemsPerPage = 0,
            string search = null,
            string sortBy = null,
            bool ascending = true)
        {
            var query = db.rendeles.OrderBy(x => x.id).AsQueryable();

            // Keresés
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                int ferohely;
                int.TryParse(search, out ferohely);

                query = query.Where(x => x.ugyfel.vezeteknev.ToLower() == search ||
                                         x.ugyfel.keresztnev.ToLower() == search ||
                                         x.ugyfel.telefonszam.ToLower() == search ||
                                         x.ugyfel.email.ToLower() == search ||
                                         x.jarmu.rendszam.ToLower() == search);
            }

            // Sorbarendezés
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy)
                {
                    default:
                        query = ascending ? query.OrderBy(x => x.id) : query.OrderByDescending(x => x.id);
                        break;
                    case "ugyfelnev":
                        query = ascending ? query.
                            OrderBy(x => x.ugyfel.vezeteknev).
                            ThenBy(x => x.ugyfel.keresztnev) : query.
                            OrderByDescending(x => x.ugyfel.vezeteknev).
                            ThenByDescending(x => x.ugyfel.keresztnev);
                        break;
                    case "telefonszam":
                        query = ascending ? query.OrderBy(x => x.ugyfel.telefonszam) : query.OrderByDescending(x => x.ugyfel.telefonszam);
                        break;
                    case "email":
                        query = ascending ? query.OrderBy(x => x.ugyfel.email) : query.OrderByDescending(x => x.ugyfel.email);
                        break;
                    case "pont":
                        query = ascending ? query.OrderBy(x => x.ugyfel.pont) : query.OrderByDescending(x => x.ugyfel.pont);
                        break;
                    case "rendszam":
                        query = ascending ? query.OrderBy(x => x.jarmu.rendszam) : query.OrderByDescending(x => x.jarmu.rendszam);
                        break;
                    case "ferohely":
                        query = ascending ? query.OrderBy(x => x.jarmu.ferohely) : query.OrderByDescending(x => x.jarmu.ferohely);
                        break;
                    case "datum":
                        query = ascending ? query.OrderBy(x => x.datum) : query.OrderByDescending(x => x.datum);
                        break;
                }
            }


            // Összes találat kiszámítása
            _totalItems = query.Count();

            // Oldaltördelés
            if (page + itemsPerPage > 0)
            {
                query = query.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);
            }

            var rendelesVMList = new List<rendelesVM>();
            var dbList = query.ToList();
            foreach (rendeles item in dbList)
            {
                var teljesNev = item.ugyfel.vezeteknev + " " + item.ugyfel.keresztnev;
                rendelesVMList.Add(new rendelesVM(
                    item.id,
                    item.ugyfel_id,
                    teljesNev, 
                    item.ugyfel.telefonszam,
                    item.ugyfel.email,
                    item.ugyfel.pont,
                    item.jarmu.Id,
                    item.jarmu.rendszam,
                    item.jarmu.ferohely,
                    item.datum));
            }
            return new BindingList<rendelesVM>(rendelesVMList);
        }

        public int Count()
        {
            return _totalItems;
        }

        // HACK: RendelesRepo Insert
        public void Insert(rendelesVM rendelesVM)
        {
            var rendeles = new rendeles(
                        rendelesVM.ugyfelId, 
                        rendelesVM.jarmuId, 
                        rendelesVM.rendelesDatum
                        );
            db.rendeles.Add(rendeles);
        }

        public void Delete(int id)
        {
            var rendeles = db.rendeles.Find(id);
            db.rendeles.Remove(rendeles);
            db.SaveChanges();
        }

        // HACK: RendelesRepo Update

        public void Update(rendelesVM rendelesVM)
        {
            var rendeles = db.rendeles.Find(rendelesVM.rendelesId);
            if (rendeles != null)
            {
                rendeles.ugyfel.id = rendelesVM.ugyfelId;
                rendeles.jarmu_id = rendelesVM.jarmuId;
                rendeles.datum = rendelesVM.rendelesDatum;
                db.Entry(rendeles).State = EntityState.Modified;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public bool Exists(rendelesVM rendelesVM)
        {
            return db.rendeles.Any(x => x.id == rendelesVM.rendelesId);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
        }
    }
}
