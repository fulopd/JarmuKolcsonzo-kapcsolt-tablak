using JarmuKolcsonzo.Models;
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
        // TODO: repok hozzáadása
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

        // TODO: Presenter Add
        public void Add(rendelesVM rendelesVM)
        {
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

        // TODO: Presenter Modify
        public void Modify(int index, rendelesVM rendelesVM)
        {
            view.bindingList[index] = rendelesVM;
            repo.Update(rendelesVM);
        }

        public void Save()
        {
            repo.Save();
            LoadData();
        }
    }
}
