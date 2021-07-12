<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AplicacionProspectos._Default"  EnableEventValidation="false" MaintainScrollPositionOnPostBack = "true" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.0/dist/css/bootstrap.min.css" integrity="sha384-B0vP5xmATw1+K9KRQjQERJvTumQW0nPEzvF6L/Z6nronJ3oUOFUFpCjEUQouq2+l" crossorigin="anonymous">
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.0/dist/js/bootstrap.bundle.min.js" integrity="sha384-Piv4xVNRyMGpqkS2by6br4gNJ7DXjqk09RmUpJ8jgGtD7zP9yug3goQfGII0yAns" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@9"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/core-js/2.4.1/core.js"></script>

    <!-- iconos -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.5.0/font/bootstrap-icons.css" />

     <script type="text/javascript">
          function mostrarModalProspecto() {
               $('#modalAgregarProspecto').modal({backdrop: 'static', keyboard: false})
                $('#modalAgregarProspecto').modal('show');

               return false;
         }
         function mostrarModalDocumentos() {
               $('#modalAgregarDocumentos').modal({backdrop: 'static', keyboard: false})
             $('#modalAgregarDocumentos').modal('show');

               return false;
         }
         function mostrarModalMensajeConfirmacion() {
               $('#modalMensajeConfirmacion').modal({backdrop: 'static', keyboard: false , weigth: 'auto', maxweight:'60px'})
                $('#modalMensajeConfirmacion').modal('show');
               return false;
         }
         function inhabilitarBoton(valor) {
             document.getElementById('btnAgregarDocumentosModal').disabled=valor;
         }

         function mensaje(msg, icono, modal) {
             if (modal !== '')
             {
                 $(modal).modal({backdrop: 'static', keyboard: false})
                $(modal).modal('show');
             }
            Swal.fire({
            icon: icono,
            title: msg,
            showConfirmButton: false,
            timer: 10000
            })
                 
         }

         function SoloNumeros(ev) {
            var key = ev.which || ev.keyCode || ev.charCode;
            if (!((key >= 48 && key <= 57) || key == 8 || key == 9 ))
                return false;
            else
                return true;
         }
          function clickBtnAgregar() {
              $("#<%= btnAgregarDocumentosGrid.ClientID %>").trigger('click');
           }
        
     </script>   

   <style type="text/css">
      .file-upload {
  cursor: pointer;      
}

.file-upload input {
}
        
        </style>

</head>
    <body>
    <form id="formProspectos" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
         <div class="container">
                <div class="row">
                    <div class="col-lg-10 m-auto m-2">
                    <h1>Listado de prospectos</h1>
                </div>
                </div>
            <div class="row form-group m-2">
                    <asp:GridView ID="gvProspectos" CssClass="table " runat="server" AutoGenerateColumns="False"  OnRowDataBound="gvProspectos_RowDataBound1" OnSelectedIndexChanged="gvProspectos_SelectedIndexChanged" OnRowCommand="gvProspectos_RowCommand">
                    <Columns>
                        <asp:BoundField HeaderText="Numero Prospecto" DataField="numeroCliente" HeaderStyle-CssClass="hidden thead-dark" ItemStyle-CssClass="hidden">
                             <HeaderStyle HorizontalAlign="Center"  Width="15%" ></HeaderStyle>
						     <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                        </asp:BoundField >
                        <asp:BoundField HeaderText="Nombre" DataField="nombre"  >
                            <HeaderStyle HorizontalAlign="Center"  Width="17%" ></HeaderStyle>
			                <ItemStyle HorizontalAlign="Left" Width="17%"></ItemStyle>
                            </asp:BoundField >
                        <asp:BoundField HeaderText="Apellido Paterno" DataField="apellidoPaterno"  >
                            <HeaderStyle HorizontalAlign="Center"  Width="17%" ></HeaderStyle>
			                <ItemStyle HorizontalAlign="Left" Width="17%"></ItemStyle>
                            </asp:BoundField >
                        <asp:BoundField HeaderText="Apellido Materno" DataField="apellidoMaterno"  >
                            <HeaderStyle HorizontalAlign="Center"  Width="17%" ></HeaderStyle>
			                <ItemStyle HorizontalAlign="Left" Width="17%"></ItemStyle>
                            </asp:BoundField >
                        <asp:BoundField HeaderText="Estatus" DataField="estatus"   >
                            <HeaderStyle HorizontalAlign="Center"  Width="17%" ></HeaderStyle>
						    <ItemStyle HorizontalAlign="Left" Width="17%"></ItemStyle>
                         </asp:BoundField >
                        <asp:BoundField HeaderText="Observaciones" DataField="observaciones"   >
                            <HeaderStyle HorizontalAlign="Center"  Width="30%" ></HeaderStyle>
						    <ItemStyle HorizontalAlign="Left" Width="30%"></ItemStyle>
                         </asp:BoundField >
                        
                    </Columns>
                    </asp:GridView>
            </div>
                <div class="row">
                    <div class="col-lg-7 col-md-7 col-sm-10 m-3">
                    <label runat="server" > Click para seleccionar el prospecto </label>
                        </div>
                </div>
                <div class="col-lg-7 col-md-7 col-sm-10 m-3">
                  <asp:Button runat="server" ID="btnAgregar" CssClass="btn btn-primary" Text="Capturar Prospecto" OnClick="btnAgregar_Click"  />
                </div>
        </div>

        <!--Modal Alta y autorizar prospectos-->
        <div class="modal fade" id="modalAgregarProspecto" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true" data-backdrop="static" data-keyboard="false" >
          <div class="modal-dialog modal-lg">
            <div class="modal-content">
              <div class="modal-header">
                <asp:Label runat="server" id="lblEncabezadoModal" CssClass="font-weight-lighter font-weight-bold" >Captura de prospectos </asp:Label> 
              </div>
              <div class="modal-body">
                  <div class="row">
                      <div class="col-md-6 ml-auto">
                          <label runat="server" class="alert-info font-weight-bold" > Todos los campos con (*) son obligatorios</label>
                      </div>
                  </div>
                  <div class="row form-group">
                   <div class="col-lg-6 col-md-6 col-sm-9">
                        <label runat="server">Numero</label> 
                        <asp:TextBox runat="server" id="txtnumeroCliente" CssClass="form-control" TextMode="Number" Enabled="false"  />
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-9">
                       <label runat="server">Nombre*</label> 
                       <asp:TextBox runat="server" id="txtnombre" CssClass="form-control"  onkeypress="if (event.keyCode==13) return false;" />
                    </div>
                      </div>
                  <div class="row form-group">
                     <div class="col-lg-6 col-md-6 col-sm-9">
                        <label runat="server">Primer Apellido*</label> 
                        <asp:TextBox runat="server" id="txtPrimerApellido"  CssClass="form-control" onkeypress="if (event.keyCode==13) return false;" />
                    </div>
                      <div class="col-lg-6 col-md-6 col-sm-9">
                        <label runat="server">Segundo Apellido</label> 
                        <asp:TextBox runat="server" id="txtSegundoApellido"   CssClass="form-control" onkeypress="if (event.keyCode==13) return false;" />
                    </div>
                </div>
                <div class="row form-group">
                    <div class="col-lg-6 col-md-6 col-sm-9">
                        <label runat="server">RFC*</label> 
                        <asp:TextBox runat="server" id="txtRfc" CssClass="form-control" onkeypress="if (event.keyCode==13) return false;"  />
                    </div>
                </div>
                <div class="row form-group">
                    <div class="col-lg-6 col-md-6 col-sm-9">
                        <label runat="server" >Calle*</label>
                        <asp:TextBox runat="server" id="txtCalle"  CssClass="form-control" onkeypress="if (event.keyCode==13) return false;"/>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-9">
                        <label runat="server" >Numero Casa</label>
                        <asp:TextBox runat="server" id="txtNumeroCasa"  CssClass="form-control" onkeypress="if (event.keyCode==13) return false;"/>
                    </div>
                </div>
                <div class="row form-group">
                    <div class="col-lg-6 col-md-6 col-sm-9">
                        <label runat="server">Colonia*</label>
                        <asp:TextBox runat="server" id="txtColonia"   CssClass="form-control" onkeypress="if (event.keyCode==13) return false;"/>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-9">
                        <label runat="server">Código Postal*</label>
                        <asp:TextBox runat="server" id="txtCodigoPostal"  CssClass="form-control" onkeypress="return SoloNumeros(event)" MaxLength="5" />
                    </div>
                </div>


                <div class="row form-group">
                    <div class="col-lg-6 col-md-6 col-sm-9">
                        <label runat="server">Telefono*</label>
                        <asp:TextBox runat="server" ID="txtTelefono"  CssClass="form-control" onkeypress="return SoloNumeros(event)"   MaxLength="10"/>
                    </div>
                   
                </div>
                <div class="row form-group">
                    <div class="col-lg-6 col-md-6 col-sm-9">
                      <label runat="server">Estatus</label>
                       <asp:DropDownList runat="server" ID="ddlEstatus"  CssClass="form-control"  />
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-9">
                       <label runat="server">Observaciones</label>
                        <asp:TextBox runat="server" ID="txtObservacion" CssClass="form-control"  Enabled="true" onkeypress="if (event.keyCode==13) return false;"/>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-9 mt-4">
                        <button runat="server" type="button" id="btnAgregarDocumentosModal"  data-dismiss="modal"  class="btn btn-primary" onclick="clickBtnAgregar();">
                          <span aria-hidden="true"><i class="bi bi-plus-circle"></i> Agregar Documentos</span> *
                        </button>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-9">
                           <asp:Button runat="server" ID="btnAgregarDocumentos" OnClick="btnAgregarDocumentos_click" class="btn btn-primary" Text="Agregar Documentos" Visible="false"/>
                        </div>
                     
                </div>
                  <div class="row">
                      <h3 runat="server">Documentos Capturdos</h3>
                  </div>
                  <div class="row form-group mt-4">
                      
                      <asp:GridView ID="gvDocumentosAltaProspecto" CssClass="table table-striped table-bodered table-hover" runat="server" AutoGenerateColumns="False" Height="97px"
                             >
                        <Columns>
                            <asp:BoundField HeaderText="Documento" DataField="nombreDocumento" >
                                 <HeaderStyle HorizontalAlign="Center"  Width="15%" ></HeaderStyle>
						         <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                            </asp:BoundField >
                            <asp:BoundField HeaderText="Nombre del archivo" DataField="nombreArchivo"  >
                                <HeaderStyle HorizontalAlign="Center"  Width="15%" ></HeaderStyle>
			                    <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                            </asp:BoundField >
                            <asp:BoundField HeaderText="Ruta" DataField="rutaArchivo"  >
                                <HeaderStyle HorizontalAlign="Center"  Width="70%" ></HeaderStyle>
			                    <ItemStyle HorizontalAlign="Left" Width="70%"></ItemStyle>
                            </asp:BoundField >
                        </Columns>
                        </asp:GridView>
                  </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-danger" OnClick="btnCancelar_click" Text="Salir"/>
                    <asp:Button runat="server" ID="btnEliminar" CssClass="btn btn-danger" OnClick="btnEliminar_click" Text="Eliminar" Visible="false"/>
                    <asp:Button runat="server" ID="btnGuardar" CssClass="btn btn-primary" OnClick="btnGuardar_click" Text="Guardar"/>
                    <asp:Button runat="server" ID="btnEvaluar" CssClass="btn btn-primary" OnClick="btnEvaluar_click" Text="Evaluar" Visible="false"/>
                </div>
           </div>
          </div>
         </div>
            </div>
        

        <!--Modal Alta y autorizar prospectos-->
        <div class="modal fade" id="modalAgregarDocumentos" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true" >
          <div class="modal-dialog modal-lg">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title" id="documentos">Alta documentos</h5>
              </div>
              <div class="modal-body">
                    
                     <div class="row form-group">
                        <div class="col-lg-6 col-md-6 col-sm-9">
                          <label runat="server">Documentos</label>
                           <asp:TextBox runat="server" ID="txtNombreDocumento"  CssClass="form-control"  />
                        </div>
                         <div class="col-lg-6 col-md-6 col-sm-9">
                            <label runat="server">Seleccione un archivo</label>
                            <asp:FileUpload id="FileUpload1" type="file" runat="server" NAME="oFile"/>
                        </div>                    
                       
                    </div>
                  
                  <div class="row form-group">
                        <div class="col-lg-2 col-md-2 col-sm-3">
                            <asp:Button runat="server" ID="btnAgregarDocumentosGrid" CssClass="btn btn-secondary" OnClick="btnAgregarDocumentosGrid_click" Text="Agregar"/>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-9 mt-2">
                            <asp:Label runat="server" ID="lblErrorDocumentos" CssClass="alert alert-danger alert-heading" Visible ="false">Los dos campos son obligatorios</asp:Label>
                        </div>
                    </div>
                        <asp:GridView ID="gridAgregarDocumentos" CssClass="table table-striped table-bodered table-hover" runat="server" AutoGenerateColumns="False" Height="97px"
                             >
                        <Columns>
                            <asp:BoundField HeaderText="Documento" DataField="nombreDocumento" >
                                 <HeaderStyle HorizontalAlign="Center"  Width="15%" ></HeaderStyle>
						         <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                            </asp:BoundField >
                            <asp:BoundField HeaderText="Nombre del archivo" DataField="nombreArchivo"  >
                                <HeaderStyle HorizontalAlign="Center"  Width="15%" ></HeaderStyle>
			                    <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                            </asp:BoundField >
                            <asp:BoundField HeaderText="Ruta" DataField="rutaArchivo"  >
                                <HeaderStyle HorizontalAlign="Center"  Width="70%" ></HeaderStyle>
			                    <ItemStyle HorizontalAlign="Left" Width="70%"></ItemStyle>
                            </asp:BoundField >
                        </Columns>
                        </asp:GridView>
                  <div class="modal-footer">
                        <asp:Button runat="server" ID="btnAceptarDocumentos" CssClass="btn btn-primary" OnClick="btnAceptarDocumentos_click" Text="Aceptar"/>
                  </div>
           </div>
          </div>
         </div>
        </div>
          <!-- Modal validacion eliminar-->

        <div class="modal fade" id="modalMensajeConfirmacion" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                <h5 class="modal-title" id="modalTitulo">Confirmación</h5>
                </div>
                <div class="modal-body">
                    ¿Esta seguro que desea cancelar?, los datos se perderán
                </div>
                <div class="modal-footer">
                
                <asp:Button runat="server" ID="btnCancelarEliminar" CssClass="btn btn-primary" OnClick="btnCancelarEliminar_click" Text="Seguir capturando "  />
                <asp:Button runat="server" ID="btnConfirmar" CssClass="btn btn-danger" OnClick="btnConfirmar_click" Text="Si, Cancelar"  />
                </div>
            </div>
            </div>
        </div>
        </form>
    </body>
</html>
