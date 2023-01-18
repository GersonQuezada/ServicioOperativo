﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Dto.Socia.Busqueda
{
    public class ListaSociaPorBancoComunalDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string BancoComunal { get; set; }
        public string AnilloGrupal { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public int? IdExterno { get; set; }
    }
}