using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AplicacionProspectos.Entidades;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace AplicacionProspectos.BD
{
    public class ConexionBD
    {
        //mongodb+srv://root:root@cluster0.1ghwu.mongodb.net/myFirstDatabase?retryWrites=true&w=majority
        public MongoClient conexionMongoDB() {

            return new MongoClient(
            "mongodb+srv://root:root@cluster0.1ghwu.mongodb.net/dbprospectos?retryWrites=true&w=majority"
            );
        }

        public List<Prospecto> consultarProspectos() {
            try
            {

                List<Prospecto> lstProspecto = new List<Prospecto>();
                MongoClient mongoClient = conexionMongoDB();

                var database = mongoClient.GetDatabase("dbprospectos");

                var collection = database.GetCollection<Prospecto>("prospectos");
                var filter = Builders<Prospecto>.Filter.Empty;
                var result = collection.Find(filter).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                   
            }
            

        }

        public ObjectId subirDocumento(string rutaArchivo, string nombreArchivo)
        {
            ObjectId id = new ObjectId() ;
            try
            {
                var database = conexionMongoDB().GetDatabase("dbprospectos");
                var fs = new GridFSBucket(database);
                id = UploadFile(fs, rutaArchivo, nombreArchivo);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           

            return id;
        }
        private static ObjectId UploadFile(GridFSBucket fs, string rutaArchivo,string nombreArchivo)
        {
            using (var s = File.OpenRead(rutaArchivo))
            {
                var t = Task.Run<ObjectId>(() => {
                    return fs.UploadFromStreamAsync(nombreArchivo, s);
                });

                return t.Result;
            }
        }

        private static void DownloadFile(GridFSBucket fs, ObjectId id)
        {
            //Consultar el archivo
            var t = fs.DownloadAsBytesByNameAsync("Captura2.PNG");
            Task.WaitAll(t);
            var bytes = t.Result;
            byte[] image = t.Result;

            FileStream archivo = new FileStream("C:\\PruebaMongodb\\Captura22.PNG", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            fs.DownloadToStream(id, archivo);
            archivo.Dispose();
        }

    }
}