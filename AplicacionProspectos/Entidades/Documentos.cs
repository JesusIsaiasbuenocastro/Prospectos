using AplicacionProspectos.BD;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplicacionProspectos.Entidades
{
    public class Documentos
    {
        public Documentos() { }
        public Documentos(int numeroCliente)
        {
            this.m_numeroClienteProspecto = numeroCliente;
        }

        #region Variables privadas
        private ObjectId m_files_id;
        private ObjectId m_id;
        private string m_descripcion;
        private int m_numeroClienteProspecto;
        private string m_nombreArchivo;
        private string m_rutaArchivo;
        private bool m_existe;
        ConexionBD conexion = new ConexionBD();
        #endregion

        #region Propiedades
        public ObjectId _id
        {
            get { return m_id; }
            set { this.m_id = value; }
        }
        public ObjectId files_id
        {
            get { return m_files_id; }
            set { this.m_files_id = value; }
        }

        
        public string descripcion
        {
            get { return m_descripcion; }
            set { this.m_descripcion = value; }
        }

        public int numeroCliente
        {
            get { return m_numeroClienteProspecto; }
            set { this.m_numeroClienteProspecto = value; }
        }
        #endregion

        public string nombreArchivo
        {
            get { return m_nombreArchivo; }
            set { this.m_nombreArchivo = value; }
        }
        public string rutaArchivo
        {
            get { return m_rutaArchivo; }
            set { this.m_rutaArchivo = value; }
        }
        public bool existe
        {
            get { return m_existe; }
            set { this.m_existe = value; }
        }

        public List<Documentos> consultarDocumentos()
        {
            try
            {

                List<Prospecto> lstProspecto = new List<Prospecto>();
                MongoClient mongoClient = conexion.conexionMongoDB();

                var database = mongoClient.GetDatabase("dbprospectos");

                var collection = database.GetCollection<Documentos>("documentosProspectos");
                var filter = Builders<Documentos>.Filter.Where(s => s.numeroCliente == m_numeroClienteProspecto);
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

                MongoClient mongoClient = conexion.conexionMongoDB();
                var database = mongoClient.GetDatabase("dbprospectos");
                var collection = database.GetCollection<BsonDocument>("documentosProspectos");
                
                var document = new BsonDocument
                {
                    { "numeroCliente", this.m_numeroClienteProspecto },
                    { "files_id", this.m_files_id },
                    { "descripcion", this.m_descripcion },
                    { "rutaArchivo", this.m_rutaArchivo },
                    { "nombreArchivo", this.m_nombreArchivo }
                };

                if (existe)
                {
                    //eLIMINAR EL REGISTRO DEL ARCHIVO BINARIO
                    collection.DeleteMany("{ numeroCliente : " + this.m_numeroClienteProspecto + " }");
                    collection.DeleteMany("{ numeroCliente : " + this.m_numeroClienteProspecto + " }");
                }
                collection.InsertOne(document);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        public List<Documentos> cargarDocumentos()
        {
            try
            {
                MongoClient mongoClient = conexion.conexionMongoDB();
                var database = mongoClient.GetDatabase("dbprospectos");
                var collection = database.GetCollection<Documentos>("documentos");


                var filter = Builders<Documentos>.Filter.Empty;
                var result = collection.Find(filter).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}