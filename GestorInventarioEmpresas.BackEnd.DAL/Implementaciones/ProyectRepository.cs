﻿using GestorInventarioEmpresas.BackEnd.DAL.Interfaces;
using GestorInventarioEmpresas.BackEnd.Domain;
using GestorInventarioEmpresas.BackEnd.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.DAL.Implementaciones
{
    public class ProyectRepository : GenericRepository<Proyect>, IProyectRepository
    {
        public ProyectRepository(DbContext context) : base(context)
        {
        }
    }
}
