using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplicacionProspectos.Entidades
{
    public class Tareas
    {
        #region Variables privadas 
        private ObjectId m_id;
        private bool m_estado;
        private string m_nombre;
        private DateTime m_fechaCreado;
        private ObjectId m_proyecto;
        #endregion

        #region Propiedades

        public ObjectId _id
        {
            get { return m_id;  }
            set { this.m_id = value;  }
        }
        public bool estado
        {
            get { return m_estado; }
            set { this.m_estado = value; }
        }
        public string nombre
        {
            get { return m_nombre; }
            set { this.m_nombre = value; }
        }
        public DateTime creado
        {
            get { return m_fechaCreado; }
            set { this.m_fechaCreado = value; }
        }
        public ObjectId proyecto
        {
            get { return m_proyecto; }
            set { this.m_proyecto = value; }
        }
        #endregion

    }
}