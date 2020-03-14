﻿using JarmuKolcsonzo.Models;
using JarmuKolcsonzo.Properties;
using JarmuKolcsonzo.Repositories;
using JarmuKolcsonzo.ViewInterfaces;
using JarmuKolcsonzo.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarmuKolcsonzo.Presenters
{
    class RendelesListaPresenter
    {
        private IDataGridList<rendelesVM> view;
        private RendelesRepository repo = new RendelesRepository();
        // HACK: repok hozzáadása
        private UgyfelRepository ugyfelRepo;
        private JarmuRepository jarmuRepo;
        

        public RendelesListaPresenter(IDataGridList<rendelesVM> param)
        {
            view = param;
        }

        public void LoadData()
        {
            view.bindingList = repo.GetAllRendelesVM(
                view.pageNumber, view.itemsPerPage, view.search, view.sortBy, view.ascending);
            view.totalItems = repo.Count();
        }

        // HACK: Presenter Add
        public void Add(rendelesVM rendelesVM)
        {
            using (ugyfelRepo = new UgyfelRepository())
            {
                var ugyfel = ugyfelRepo.GetUgyfelByName(rendelesVM.ugyfelNev);
                rendelesVM.ugyfelId = ugyfel.id;
                rendelesVM.ugyfelTelefonszam = ugyfel.telefonszam;
                rendelesVM.ugyfelEmail = ugyfel.email;
                rendelesVM.ugyfelPont = ugyfel.pont;
            }

            using (jarmuRepo = new JarmuRepository())
            {
                var jarmu = jarmuRepo.GetJarmuByLicensePlate(rendelesVM.jarmuRendszam);
                rendelesVM.jarmuId = jarmu.Id;
                rendelesVM.jarmuFerohely = jarmu.ferohely;
            }
            view.bindingList.Add(rendelesVM);
            repo.Insert(rendelesVM);
        }

        public void Remove(int index)
        {
            var rendeles = view.bindingList.ElementAt(index);
            view.bindingList.RemoveAt(index);
            if (rendeles.rendelesId > 0)
            {
                repo.Delete(rendeles.rendelesId);
            }
        }

        // HACK: Presenter Modify
        public void Modify(int index, rendelesVM rendelesVM)
        {
            using (ugyfelRepo = new UgyfelRepository())
            {
                var ugyfel = ugyfelRepo.GetUgyfelByName(rendelesVM.ugyfelNev);
                if (ugyfel != null)
                {
                    rendelesVM.ugyfelId = ugyfel.id;
                    rendelesVM.ugyfelTelefonszam = ugyfel.telefonszam;
                    rendelesVM.ugyfelEmail = ugyfel.email;
                    rendelesVM.ugyfelPont = ugyfel.pont;
                }
                
            }

            using (jarmuRepo = new JarmuRepository())
            {
                var jarmu = jarmuRepo.GetJarmuByLicensePlate(rendelesVM.jarmuRendszam);
                if (jarmu != null)
                {
                    rendelesVM.jarmuId = jarmu.Id;
                    rendelesVM.jarmuFerohely = jarmu.ferohely;
                }
                
            }
            if (rendelesVM.ugyfelId > 0 && rendelesVM.jarmuId >0)
            {
                view.bindingList[index] = rendelesVM;
                repo.Update(rendelesVM);
            }
            
        }

        public void Save()
        {
            repo.Save();
            LoadData();
        }
    }
}
