﻿using JarmuKolcsonzo.Properties;
using JarmuKolcsonzo.Repositories;
using JarmuKolcsonzo.ViewInterfaces;
using JarmuKolcsonzo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace JarmuKolcsonzo.Presenters
{
    public class RendelesPresenter
    {
        IRendelesView view;
        RendelesRepository repo = new RendelesRepository();
        private UgyfelRepository ugyfelRepo = new UgyfelRepository();
        private JarmuRepository jarmuRepo = new JarmuRepository();

        public RendelesPresenter(IRendelesView param)
        {
            view = param;
        }

        public void Save(rendelesVM rendelesVM)
        {
            view.errorUgyfelNev = null;
            view.errorJarmuRendszam = null;

            if (string.IsNullOrEmpty(rendelesVM.ugyfelNev))
            {
                view.errorUgyfelNev = Resources.KotelezoMezo;
            }
            if (string.IsNullOrEmpty(rendelesVM.jarmuRendszam))
            {
                view.errorJarmuRendszam = Resources.KotelezoMezo;
            }
            // HACK: Ellenőrizni, hogy létezik e az Ügyfél és a Jármű
            if (ugyfelRepo.GetUgyfelByName(rendelesVM.ugyfelNev)==null)
            {
                view.errorUgyfelNev = Resources.NemUgyfel;
            }
            if (jarmuRepo.GetJarmuByLicensePlate(rendelesVM.jarmuRendszam)==null)
            {
                view.errorJarmuRendszam = Resources.NemJarmu;
            }
        }
    }
}
