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
                        break;
                    case "telefonszam":
                        break;
                    case "email":
                        break;
                    case "pont":
                        break;
                    case "rendszam":
                        break;
                    case "ferohely":
                        break;
                    case "datum":
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

            return new BindingList<rendelesVM>(rendelesVMList);
        }

        public int Count()
        {
            return _totalItems;
        }

        // TODO: RendelesRepo Insert
        public void Insert(rendelesVM rendelesVM)
        {

        }

        public void Delete(int id)
        {
            var rendeles = db.rendeles.Find(id);
            db.rendeles.Remove(rendeles);
            db.SaveChanges();
        }

        // TODO: RendelesRepo Update

        public void Update(rendelesVM rendelesVM)
        {

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
