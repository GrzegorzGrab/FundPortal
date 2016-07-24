using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Repozytorium.Models.Navigation
{
    public class Navigator
    {
        private Collection<string> _Odnosniki;
        private Collection<string> _Adresy;
        public Navigator(string[] odnosniki)//, string[] adresy) 
        {
            _Odnosniki = new Collection<string>(odnosniki);
            //_Adresy = new Collection<string>(adresy);
        }

        public Collection<string> Odnosniki
        {
            get { return _Odnosniki; }
        }
    }
}