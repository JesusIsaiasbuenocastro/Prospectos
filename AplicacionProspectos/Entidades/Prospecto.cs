using AplicacionProspectos.BD;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplicacionProspectos.Entidades
{
    public class Prospecto
    {
        #region Constructor
        public Prospecto()
        {
        }
        public Prospecto( int numeroProspecto)
        {
            this.m_NumeroCliente = numeroProspecto;
        }

        #endregion

        #region Variables privadas 
        private ObjectId m_id;
        private int m_NumeroCliente;
        private string m_nombre;
        private string m_apellidoPaterno;
        private string m_apellidoMaterno;
        private string m_calle;
        private string m_numeroDireccion;
        private string m_colonia;
        private int m_codigoPostal;
        private string m_telefono;
        private string m_rfc;
        private string m_estatus;
        private string m_observaciones;
        private bool m_existe;
        ConexionBD conexion = new ConexionBD();

        #endregion

        #region Propiedades

        public ObjectId _id
        {
            get { return m_id; }
            set { this.m_id = value; }
        }

        public int numeroCliente
        {
            get { return m_NumeroCliente; }
            set { this.m_NumeroCliente = value; }
        }

        public string nombre
        {
            get { return m_nombre; }
            set { this.m_nombre = value; }
        }
        public string apellidoPaterno
        {
            get { return m_apellidoPaterno; }
            set { this.m_apellidoPaterno = value; }
        }
        public string apellidoMaterno
        {
            get { return m_apellidoMaterno; }
            set { this.m_apellidoMaterno = value; }
        }
        public string calle
        {
            get { return m_calle; }
            set { this.m_calle = value; }
        }
        public string numeroDireccion
        {
            get { return this.m_numeroDireccion; }
            set { this.m_numeroDireccion = value; }
        }
        public string colonia
        {
            get { return m_colonia; }
            set { this.m_colonia = value; }
        }
        public int codigoPostal
        {
            get { return m_codigoPostal; }
            set { this.m_codigoPostal = value; }
        }
        public string telefono
        {
            get { return m_telefono; }
            set { this.m_telefono = value; }
        }
        public string rfc
        {
            get { return m_rfc; }
            set { this.m_rfc = value; }
        }
        public string estatus
        {
            get { return m_estatus; }
            set { this.m_estatus = value; }
        }
        public string observaciones
        {
            get { return m_observaciones; }
            set { this.m_observaciones = value; }
        }
        public bool existe
        {
            get { return m_existe; }
            set { this.m_existe = value; }
        }


        #endregion

        #region Metodos Publicos
        public List<Prospecto> consultarProspectos()
        {
            try
            {

                List<Prospecto> lstProspecto = new List<Prospecto>();
                MongoClient mongoClient = conexion.conexionMongoDB();

                var database = mongoClient.GetDatabase("dbprospectos");

                var collection = database.GetCollection<Prospecto>("prospectos");
                var filter = Builders<Prospecto>.Filter.Where(s => s.numeroCliente == m_NumeroCliente);
                var result = collection.Find(filter).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }


        }
        public void guardar()
        {
            try
            {

                List<BsonDocument> lstProspecto = new List<BsonDocument>();
                MongoClient mongoClient = conexion.conexionMongoDB();
                var database = mongoClient.GetDatabase("dbprospectos");
                var collection = database.GetCollection<BsonDocument>("prospectos");



                var document = new BsonDocument
                    {
                        { "nombre", this.m_nombre },
                        { "apellidoPaterno", this.m_apellidoPaterno },
                        { "apellidoMaterno", this.m_apellidoMaterno },
                        { "calle", this.m_calle },
                        { "numeroDireccion", this.m_numeroDireccion },
                        { "colonia", this.m_colonia},
                        { "codigoPostal", this.m_codigoPostal },
                        { "rfc", this.m_rfc},
                        { "numeroCliente", this.m_NumeroCliente},
                        { "telefono", this.m_telefono},
                        { "estatus", this.m_estatus},
                        { "observaciones", this.m_observaciones}

                    };

                if (existe)
                {
                    collection.DeleteMany("{ numeroCliente : "+this.m_NumeroCliente + " }");
                }                
                collection.InsertOne(document);

            }
            catch (Exception ex)
            {
              
                throw new Exception(ex.Message);

            }
        }

        #endregion




    }
}