var firstDate;
var lastDate;
var retCompanies;
var EditableTable;
var dataEditTable;
var needChange = true;
var Lunes    =0;
var Martes   =0;
var Miercoles=0;
var Jueves   =0;
var Viernes  =0;
var Sabado   =0;
var Domingo = 0;
var cmbProyecto;
(function ($) {

    'use strict';

    EditableTable = {

        options: {
            addButton: '#addToTable',
            table: '#datatable-editable',
            dialog: {
                wrapper: '#dialog',
                cancelButton: '#dialogCancel',
                confirmButton: '#dialogConfirm',
            }
        },

        initialize: function () {
            this
                .setVars()
                .build()
                .events();
        },

        setVars: function () {
            this.$table = $(this.options.table);
            this.$addButton = $(this.options.addButton);

            // dialog
            this.dialog = {};
            this.dialog.$wrapper = $(this.options.dialog.wrapper);
            this.dialog.$cancel = $(this.options.dialog.cancelButton);
            this.dialog.$confirm = $(this.options.dialog.confirmButton);

            return this;
        },

        build: function () {
            this.datatable = this.$table.DataTable({
                "drawCallback": function( settings ) {
                    var TotalDias = [];
                    TotalDias[0] = 0;
                    TotalDias[1] = 0;
                    TotalDias[2] = 0;
                    TotalDias[3] = 0;
                    TotalDias[4] = 0;
                    TotalDias[5] = 0;
                    TotalDias[6] = 0;


                    $.each($("#datatable-editable").DataTable().data(), function (key, value) {
                         
                        TotalDias[0] = parseFloat(value[3]) + parseFloat(TotalDias[0]);
                        TotalDias[1] = parseFloat(value[4]) + parseFloat(TotalDias[1]);
                        TotalDias[2] = parseFloat(value[5]) + parseFloat(TotalDias[2]);
                        TotalDias[3] = parseFloat(value[6]) + parseFloat(TotalDias[3]);
                        TotalDias[4] = parseFloat(value[7]) + parseFloat(TotalDias[4]);
                        TotalDias[5] = parseFloat(value[8]) + parseFloat(TotalDias[5]);
                        TotalDias[6] = parseFloat(value[9]) + parseFloat(TotalDias[6]);
                    });
                    $("#TotalLunes").html(TotalDias[0]);
                    $("#TotalMartes").html(TotalDias[1]);
                    $("#TotalMiercoles").html(TotalDias[2]);
                    $("#TotalJueves").html(TotalDias[3]);
                    $("#TotalViernes").html(TotalDias[4]);
                    $("#TotalSabado").html(TotalDias[5]);
                    $("#TotalDomingo").html(TotalDias[6]);
                   
                },
                columnDefs: [
    {
        targets: [3, 4, 5, 6, 7, 8, 9],
        className: 'text-right'
    }
                ],    aoColumns: [
                    null,
                    null,
                    null,
                  { "bSortable": false },
                  { "bSortable": false },
                  { "bSortable": false },
                  { "bSortable": false },
                  { "bSortable": false },
                  { "bSortable": false },
                    { "bSortable": false },
                    { "bSortable": false }
                ],
                language: {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar _MENU_ registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                }
            });

            window.dt = this.datatable;

            return this;
        },

        events: function () {
            var _self = this;

            this.$table
                .on('click', 'a.save-row', function (e) {
                
                    BlockUIManager.block("body");
                    e.preventDefault();
                    _self.rowSave($(this).closest('tr'));
                    BlockUIManager.unblock("body");
                })
                .on('click', 'a.cancel-row', function (e) {
                    BlockUIManager.block("body");
                    _self.bloquearBotones(false);
                    e.preventDefault();
                    _self.rowCancel($(this).closest('tr'));
                    BlockUIManager.unblock("body");
                })
                .on('click', 'a.edit-row', function (e) {
                    BlockUIManager.block("body");
                    _self.bloquearBotones(true);
                    e.preventDefault();
                    _self.rowEdit($(this).closest('tr'));
                    BlockUIManager.unblock("body");
                })
                .on('click', 'a.remove-row', function (e) {
                    e.preventDefault();

                    var $row = $(this).closest('tr');
                    swal({
                        title: "Atención?",
                        text: "Estas a punto de eliminar el registro?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Si, eliminarlo!",
                        cancelButtonText: "No, cancelar!",
                        closeOnConfirm: false,
                        closeOnCancel: false
                    }, function (isConfirm) {
                        if (isConfirm) {
                            EditableTable.rowRemove($row);
                            $('#showSweetAlert').modal('toggle');
                            swal.close()
                        } else {
                            swal.close()
                        }
                    });
                });

            this.$addButton.on('click', function (e) {
                BlockUIManager.block("body");
                _self.bloquearBotones(true);
                e.preventDefault();
                _self.rowAdd();
                BlockUIManager.unblock("body");
            });

            this.dialog.$cancel.on('click', function (e) {
                BlockUIManager.block("body");
                e.preventDefault();
                _self.bloquearBotones(false);
                $.magnificPopup.close();
                BlockUIManager.unblock("body");
            });

            return this;
        },

        // ==========================================================================================
        // ROW FUNCTIONS
        // ==========================================================================================
        rowAdd: function () {
              this.$addButton.attr({ 'disabled': 'disabled' });

            var actions,
                data,
                $row;

            actions = [
                '<a href="#" class="hidden on-editing save-row  btn btn-success waves-effect waves-light"><i class="md-save"></i></a>',
                '<a href="#" class="hidden on-editing cancel-row  btn btn-danger waves-effect waves-light"><i class=" md-clear"></i></a>',
                '<a href="#" class="on-default edit-row btn btn-primary waves-effect waves-light"><i class=" md-edit"></i></a>',
                '<a href="#" class="on-default remove-row  btn btn-danger waves-effect waves-light"><i class=" md-delete"></i></a>'
            ].join(' ');

            data = this.datatable.row.add(['', '', '', 0, 0, 0, 0, 0,0,0, actions]);
            $row = this.datatable.row(data[0]).nodes().to$();

            $row
                .addClass('adding')
                .find('td:last')
                .addClass('actions');


            $row
                .find('td:nth-child(2)')
                .addClass('cmbEmpresa');
            $row
                .find('td:nth-child(1)')
                .addClass('cmbProyecto');
            $row
                .find('td:nth-child(3)')
                .addClass('cmbTipoTarea');
            $row
              .find('td:nth-child(4)')
              .addClass('Lunes');
            $row
               .find('td:nth-child(5)')
               .addClass('Martes');
            $row
               .find('td:nth-child(6)')
               .addClass('Miercoles');
            $row
               .find('td:nth-child(7)')
               .addClass('Jueves');
            $row
               .find('td:nth-child(8)')
               .addClass('Viernes');
            $row
               .find('td:nth-child(9)')
               .addClass('Sabado');
            $row
               .find('td:nth-child(10)')
               .addClass('Domingo');
            this.rowEdit($row);

            this.datatable.order([0, 'asc']).draw(); // always show fields
            $(".Sabado").css("background", "#f8ff9194");
            $(".Domingo").css("background", "#f8ff9194");

        },
        rowAddWitchData: function (V) {
            $("#datatable-editable > tbody .Sabado").css("background", "#f8ff9194");
            var actions,
                data,
                $row;

            actions = [
                '<a href="#" title="Guarar el registro actual" class="hidden on-editing save-row  btn btn-success waves-effect waves-light"><i class="md-save"></i></a>',
                '<a href="#" title="Cancelar la operación actual" class="hidden on-editing cancel-row  btn btn-danger waves-effect waves-light"><i class=" md-clear"></i></a>',
                '<a href="#" title="Editar la fila" class="on-default edit-row btn btn-primary waves-effect waves-light"><i class=" md-edit"></i></a>',
                '<a href="#" title="Eliminiar fila" class="on-default remove-row  btn btn-danger waves-effect waves-light"><i class=" md-delete"></i></a>'
            ].join(' ');

            data = this.datatable.row.add([V.NombreProyecto, V.NombreEmpresas, V.NombreTipoTarea, V.Lunes, V.Martes, V.Miercoles, V.Jueves, V.Viernes, V.Sabado, V.Domingo, actions]);

            $row = this.datatable.row(data[0]).nodes().to$();

            $row
                .find('td:last')
                .addClass('actions');

            $row
                .find('td:nth-child(1)')
                .addClass('cmbProyecto')
                .attr('data-proyectId', V.proyect);

            $row
                .find('td:nth-child(2)')
                .addClass('cmbEmpresa')
                .attr('data-company', V.company);

            $row
                .find('td:nth-child(3)')
                .addClass('cmbTipoTarea')
                .attr('data-TaskType', V.TaskType);;
            $row
               .find('td:nth-child(4)')
               .addClass('Lunes');
            $row
               .find('td:nth-child(5)')
               .addClass('Martes');
            $row
               .find('td:nth-child(6)')
               .addClass('Miercoles');
            $row
               .find('td:nth-child(7)')
               .addClass('Jueves');
            $row
               .find('td:nth-child(8)')
               .addClass('Viernes');
            $row
               .find('td:nth-child(9)')
               .addClass('Sabado');
            $row
               .find('td:nth-child(10)')
               .addClass('Domingo');


            this.datatable.order([0, 'asc']).draw(); // always show fields
            $(".Sabado").css("background", "#f8ff9194");
            $(".Domingo").css("background", "#f8ff9194");

        },
        rowCancel: function ($row) {
            var _self = this,
                $actions,
                i,
                data;

            if ($row.hasClass('adding')) {
                getWorkDaysForWeek(firstDate);
            } else {

                data = this.datatable.row($row.get(0)).data();
                this.datatable.row($row.get(0)).data(data);

                $actions = $row.find('td.actions');
                if ($actions.get(0)) {
                    this.rowSetActionsDefault($row);
                }
                getWorkDaysForWeek(firstDate);
               // this.datatable.draw();
            }
        },

        rowEdit: function ($row) {
         
            var _self = this,
                data;

            data = this.datatable.row($row.get(0)).data();
            dataEditTable = data;
            $row.children('td').each(function (i) {
                var $this = $(this);

                if ($this.hasClass('actions')) {
                    _self.rowSetActionsEditing($row);
                }
                else if ($this.hasClass('cmbProyecto')) {

                    $.ajax({

                        url: "/Projects/getAllProyects", success: function (result) {
                            var htmlSelectEmpresa = '<select class="sselect2-container select2 form-control" id="cmbProyecto"  style="background: white; min-width: 104px">';

                            $.each(result, function (key, value) {
                                htmlSelectEmpresa += '<option value="' + value.Id + '">' + value.Name + '</option>';
                            });
                            htmlSelectEmpresa += ' </select>';

                            $this.html(htmlSelectEmpresa);

                            $("#cmbProyecto").select2();
                            if ($this.data('proyectid') === undefined) {
                                $("#cmbProyecto").select2();
                            } else {
                                $("#cmbProyecto").select2().select2('val', $this.data('proyectid'));
                            }
                        }
                    });
                   
                    $(document.body).on("change", "#cmbProyecto", function () {
                        if ($this.is(":visible")) {
                            $.ajax({
                                url: "/Projects/getAllCompaniesForCompany",
                                data: { "id": $("#cmbProyecto").val() },
                                success: function (result) {
                                    var htmlSelectEmpresa;
                                    if (result.empresas != undefined) {
                                        htmlSelectEmpresa = result.empresas;
                                        $this.next().html(htmlSelectEmpresa);
                                    } else {
                                        htmlSelectEmpresa = '  <select class="sselect2-container select2 form-control" id="cmbEmpresa" selected="selected"  style="background: white;min-width: 104px">';
                                      
                                        $.each(result, function (key, value) {
                                            htmlSelectEmpresa += '<option value="' + value.Id + '">' + value.Name + '</option>';
                                        });
                                        htmlSelectEmpresa += ' </select>';
                                        $this.next().html(htmlSelectEmpresa);
                                        $("#cmbEmpresa").select2({
                                            language: UtilJs.language
                                        });
                                        if (needChange) {
                                            needChange = false;

                                            //  var dataCompanies = $this.next().data('companies');
                                            //if (dataCompanies != undefined) {
                                            //    if (!isNaN(dataCompanies)) {
                                            //        dataCompanies = UtilJs.toInt(dataCompanies);
                                            //    } else {
                                            //        dataCompanies = dataCompanies.split(",");
                                            //    }
                                            $("#cmbEmpresa").select2().select2('val', $this.next().data('company'));
                                            $("#cmbEmpresa").select2().val($this.next().data('company')).trigger('change.select2');
                                            // $("#cmbEmpresa").val(dataCompanies).trigger('change');
                                        } else {
                                            setTimeout(function () {  $("#cmbEmpresa").select2().val(0).change();
                                                $('#cmbEmpresa').val($("#target option:first").val()).trigger('change.select2');},  500);
                                        }
                                     
                                       
                                       
                                    }
                                   
                                }
                            });
                      
                            
                            if ($("#cmbProyecto").val() != undefined) {
                                $.ajax({

                                    url: "/Projects/getAllTypeTasks",
                                    data: { "id": $("#cmbProyecto").val() },
                                    success: function (result) {
                                        var htmlSelectEmpresa = '<select class="select2-container select2 form-control" id="cmbTipoTarea"  style="background: white;min-width: 104px">';

                                        $.each(result, function (key, value) {
                                            htmlSelectEmpresa += '<option value="' + value.Id + '">' + value.Name + '</option>';
                                        });
                                        htmlSelectEmpresa += ' </select>';

                                        $this.next().next().html(htmlSelectEmpresa);

                                        if ($this.next().next().data('tasktype') == undefined) {
                                            $("#cmbTipoTarea").select2();
                                        } else {
                                            $("#cmbTipoTarea").select2().select2('val', $this.next().next().data('tasktype'));
                                        }

                                    }
                                });
                            }

                        }
                    });

                    _self.rowSetActionsEditing($row);

                }
                else if ($this.hasClass('cmbEmpresa')) {


                    var htmlSelectEmpresa = '<select class="js-example-basic-multiple" id="cmbEmpresa"  style="background: white;min-width: 104px">';

                    htmlSelectEmpresa += '<option value=""> </option>';

                    htmlSelectEmpresa += ' </select>';

                    $this.html(htmlSelectEmpresa);


                    $("#cmbEmpresa").select2();
                    needChange = true;

                    //$("#cmbEmpresa").val($this.data('companies')).trigger('change');
                }
                else if ($this.hasClass('cmbTipoTarea')) {
                    if ($("#cmbProyecto").val() != undefined) {
                        $.ajax({

                            url: "/Projects/getAllTypeTasks",
                            data: { "id": $("#cmbProyecto").val() },
                            success: function (result) {
                                var htmlSelectEmpresa = '<select class="sselect2-container select2 form-control" id="cmbTipoTarea"  style="background: white; min-width: 104px">';

                                $.each(result, function (key, value) {
                                    htmlSelectEmpresa += '<option value="' + value.Id + '">' + value.Name + '</option>';
                                });
                                htmlSelectEmpresa += ' </select>';

                                $this.html(htmlSelectEmpresa);

                                if ($this.data('tasktype') == undefined) {
                                    $("#cmbTipoTarea").select2();
                                } else {
                                    $("#cmbTipoTarea").select2().select2('val', $this.data('tasktype'));
                                }

                            }
                        });
                    }
                  

                }

                else {
                    $this.html('<input type="number" min="0" max="8" class="form-control input-block" value="' + data[i] + '"/>');
                }
            });
            setTimeout(function () { $("#cmbProyecto").trigger("change"); }, 500);

        },

        rowSave: function ($row) {

          
            if ($("#cmbEmpresa").length & $("#cmbEmpresa").val() == null) {
                $.Notification.autoHideNotify('error', 'top right', 'Error', 'Debe seleccionar una empresa para poder guardar el registro');
                $row.find('.on-editing').removeClass('hidden');
                $row.find('.on-default').addClass('hidden');
                
            }else{
            var _self = this,
                $actions,
                values = [];

            if ($row.hasClass('adding')) {
                this.$addButton.removeAttr('disabled');
                $row.removeClass('adding');
            }

            values = $row.find('td').map(function () {
                var $this = $(this);

                if ($this.hasClass('actions')) {
                    _self.rowSetActionsDefault($row);
                    return _self.datatable.cell(this).data();
                } else if ($this.hasClass('cmbProyecto')) {
                    
                    return {
                        currentProyect: $.trim($("#cmbProyecto").select2('data').id),
                        lastPoryect: $this.data('proyectid')
                    };
                }
                else if ($this.hasClass('cmbTipoTarea')) {  
                    return {
                        currentTaskType: $.trim($("#cmbTipoTarea").select2('data').id),
                        lastTaskType: $this.data('tasktype')
                    };
                    
                }
                else if ($this.hasClass('cmbEmpresa')) {
                    return {
                        company: $.trim($("#cmbEmpresa").select2('data').id),
                        companyOld: $this.data('company')
                    };
                } else if ($this.hasClass('Lunes')) {
                    Lunes = $.trim($this.find('input').val());
                    return $.trim($this.find('input').val());
                } else if ($this.hasClass('Martes')) {
                    Martes = $.trim($this.find('input').val());
                    return $.trim($this.find('input').val());
                } else if ($this.hasClass('Miercoles')) {
                    Miercoles = $.trim($this.find('input').val());
                    return $.trim($this.find('input').val());
                } else if ($this.hasClass('Jueves')) {
                    Jueves = $.trim($this.find('input').val());
                    return $.trim($this.find('input').val());
                } else if ($this.hasClass('Viernes')) {
                    Viernes = $.trim($this.find('input').val());
                    return $.trim($this.find('input').val());
                } else if ($this.hasClass('Sabado')) {
                    Sabado = $.trim($this.find('input').val());
                    return $.trim($this.find('input').val());
                } else if ($this.hasClass('Domingo')) {
                    Domingo = $.trim($this.find('input').val());
                    return $.trim($this.find('input').val());
                } else {
                    return $.trim($this.find('input').val());
                }
               
            });
            
            var obj = {
                proyect: values[0].currentProyect,
                proyectOld: values[0].lastPoryect,
                company: values[1].company,
                companyOld: values[1].companyOld,
                TaskType:values[2].currentTaskType,
                TaskTypeOld: values[2].lastTaskType,
                Lunes:    Lunes,        
                Martes:   Martes,       
                Miercoles:Miercoles ,
                Jueves:   Jueves,      
                Viernes:  Viernes,      
                Sabado:   Sabado,      
                Domingo:  Domingo,     
                StartLoadString: firstDate,
                StartLoad:"01-01-2000"
            };
            if ((obj.Lunes + obj.Martes + obj.Miercoles + obj.Jueves + obj.Viernes + obj.Sabado + obj.Domingo) == 0) {

                $.Notification.autoHideNotify('error', 'top right', 'Error', 'Debe ingresar horas para poder guardar el registro');
                $row.find('.on-editing').removeClass('hidden');
                $row.find('.on-default').addClass('hidden');
                return;
            }
            //if ((parseFloat(obj.Lunes) + parseFloat($("#TotalLunes").text()) - dataEditTable[3]) > 8 ||
            //    (parseFloat(obj.Martes) + parseFloat($("#TotalMartes").text()) - dataEditTable[4]) > 8 ||
            //    (parseFloat(obj.Miercoles) + parseFloat($("#TotalMiercoles").text()) - dataEditTable[5]) > 8 ||
            //    (parseFloat(obj.Jueves) + parseFloat($("#TotalJueves").text()) - dataEditTable[6]) > 8 ||
            //    (parseFloat(obj.Viernes) + parseFloat($("#TotalViernes").text()) - dataEditTable[7]) > 8 ||
            //    (parseFloat(obj.Sabado) + parseFloat($("#TotalSabado").text()) - dataEditTable[8]) > 8 ||
            //    (parseFloat(obj.Domingo) + parseFloat($("#TotalDomingo").text()) - dataEditTable[9]) > 8) {

            //    $.Notification.autoHideNotify('error', 'top right', 'Error', 'No puede trabajar más de 8 hs al dia');
            //    $row.find('.on-editing').removeClass('hidden');
            //    $row.find('.on-default').addClass('hidden');

            //    return;
            //}
            BlockUIManager.block("#wrapper");
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                url: "SaveWeek",
                data: JSON.stringify(obj),
                type: "POST",
                success: function (result) {
             
                    if (result == "true") {
                        $.Notification.autoHideNotify('success', 'top right', 'Registro correcto', 'Se realizó correctamente su carga de horas');
                    } else {
                        $.Notification.autoHideNotify('error', 'top right', 'Error', 'Ya existe un carga de horas para ese proyecto con esas empresas y tipo de tarea asociada');
                    }
                    EditableTable.bloquearBotones(false)
                    getWorkDaysForWeek(firstDate);
                }
            }).done(function () {

                BlockUIManager.unblock("#wrapper");
            });
            this.datatable.row($row.get(0)).data(values);
            $actions = $row.find('td.actions');
            if ($actions.get(0)) {
                this.rowSetActionsDefault($row);
            }
         
            //   this.datatable.draw();
        } },

        rowRemove: function ($row) {
            var _self = this,
                 $actions,
                 values = [];
            values = $row.find('td').map(function () {
                var $this = $(this);

                if ($this.hasClass('actions')) {
                    _self.rowSetActionsDefault($row);
                    return _self.datatable.cell(this).data();
                } else if ($this.hasClass('cmbProyecto')) {
                    
                    return {
                        currentProyect: $.trim($("#cmbProyecto").select2('data').id),
                        lastPoryect: $this.data('proyectid')
                    };
                }
                else if ($this.hasClass('cmbTipoTarea')) {  
                    return {
                        currentTaskType: $.trim($("#cmbTipoTarea").select2('data').id),
                        lastTaskType: $this.data('tasktype')
                    };
                    
                }
                else if ($this.hasClass('cmbEmpresa')) {
                    return {
                        currentCompanies: $.trim($("#cmbEmpresa").select2('data').id),
                        lastCompanies: $this.data('company')
                    };
                   
                } else if ($this.hasClass('Lunes')) {
                    Lunes = $.trim($this.find('input').val());
                    return $.trim($this.find('input').val());
                } else if ($this.hasClass('Martes')) {
                    Martes = $.trim($this.find('input').val());
                    return $.trim($this.find('input').val());
                } else if ($this.hasClass('Miercoles')) {
                    Miercoles = $.trim($this.find('input').val());
                    return $.trim($this.find('input').val());
                } else if ($this.hasClass('Jueves')) {
                    Jueves = $.trim($this.find('input').val());
                    return $.trim($this.find('input').val());
                } else if ($this.hasClass('Viernes')) {
                    Viernes = $.trim($this.find('input').val());
                    return $.trim($this.find('input').val());
                } else if ($this.hasClass('Sabado')) {
                    Sabado = $.trim($this.find('input').val());
                    return $.trim($this.find('input').val());
                } else if ($this.hasClass('Domingo')) {
                    Domingo = $.trim($this.find('input').val());
                    return $.trim($this.find('input').val());
                } else {
                    return $.trim($this.find('input').val());
                }
                
            });
            
            var obj = {
                proyect: values[0].currentProyect,
                proyectOld: values[0].lastPoryect,
                company: values[1].currentCompanies,
                companyOld: values[1].lastCompanies,
                TaskType:values[2].currentTaskType,
                TaskTypeOld: values[2].lastTaskType,
                Lunes:    Lunes,        
                Martes:   Martes,       
                Miercoles:Miercoles ,
                Jueves:   Jueves,      
                Viernes:  Viernes,      
                Sabado:   Sabado,      
                Domingo:  Domingo,     
                StartLoadString: firstDate
            };
          
            if ($row.hasClass('adding')) {
                this.$addButton.removeAttr('disabled');
                $row.removeClass('adding');
            }
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                url: "deleteWeek",
                data: JSON.stringify(obj),
                type: "POST",
                success: function (result) {
             
                    if (result == "true") {
                        $.Notification.autoHideNotify('success', 'top right', 'Eliminación correcta', 'Se elimino las horas correctamente');
                    } else {
                        $.Notification.autoHideNotify('error', 'top right', 'Error', 'Ya existe un carga de horas para ese proyecto con esas empresas y tipo de tarea asociada');
                    }
                    EditableTable.bloquearBotones(false)
                    getWorkDaysForWeek(firstDate);
                }
            });
            this.datatable.row($row.get(0)).data(values);
            $actions = $row.find('td.actions');
            if ($actions.get(0)) {
                this.rowSetActionsDefault($row);
            }
         
            //   this.datatable.draw();
        },
          
        rowSetActionsEditing: function ($row) {
            $row.find('.on-editing').removeClass('hidden');
            $row.find('.on-default').addClass('hidden');
        },

        rowSetActionsDefault: function ($row) {
            $row.find('.on-editing').addClass('hidden');
            $row.find('.on-default').removeClass('hidden');
        },
        bloquearBotones: function (boolean) {
            if (boolean) {
                $("a.edit-row").css({
                    "pointer-events": "none"
                });
                $("a.remove-row").css({
                    "pointer-events": "none"
                });
                
                $("a.edit-row").attr('disabled', 'disabled');
                $("#addToTable").attr('disabled', 'disabled');
                $("a.remove-row").attr('disabled', 'disabled');

            } else {
                $("a.edit-row").css({
                    "pointer-events": "auto"
                });
                $("a.remove-row").css({
                    "pointer-events": "auto"
                });
                $("a.edit-row").removeAttr('disabled');
                $("#addToTable").removeAttr('disabled');
                $("a.remove-row").removeAttr('disabled');
            }
            
            
             
        }

    };

    $(function () {
        EditableTable.initialize();
    });

}).apply(this, [jQuery]);
$(document).ready(function () {
    BlockUIManager.block("#wrapper")
    moment.locale('en', {
        week: { dow: 1 } // Monday is the first day of the week
    });
    $("#weeklyDatePicker").datepicker({
        format: 'dd-mm-yyyy' 
    }); 
     
    addCommonListener({
        selector: ':input[type="number"]',
        name: "keyup",
        callBack: function (event) {
            $(this).val($(this).val().replace(/[^0-9\.]/g, ''));  //this.value = this.value.replace(/[^0-9\.]/g,'');
            if ($(this).val() > 8) {
                $(this).val(8);
                event.preventDefault();
            }
            $(this).val($(this).val().replace(/[^0-9\.]/g, ''));

            if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
                event.preventDefault();
            } else {
                console.log($(this).val());
                if ($(this).val() > 8) {
                    $(this).val(8);
                    event.preventDefault();
                }
            }
        }
    });
    

          //Get the value of Start and End of Week
            addCommonListener({
                selector: "#weeklyDatePicker ",
                name: "change",
                callBack: function () {

                    BlockUIManager.block("#wrapper");
                    var value = jQuery("#weeklyDatePicker").val();

                    if (value.length == 10) {
                        if (firstDate != moment(value, "DD-MM-YYYY").day(1).format("DD-MM-YYYY")) {
                            firstDate = moment(value, "DD-MM-YYYY").day(1).format("DD-MM-YYYY");
                            lastDate = moment(value, "DD-MM-YYYY").day(7).format("DD-MM-YYYY");
                            $("#Lunes").text("L " + moment(value, "DD-MM-YYYY").day(1).format("DD/MM"));
                            $("#Martes").text("M " + moment(value, "DD-MM-YYYY").day(2).format("DD/MM"));
                            $("#Miercoles").text("M " + moment(value, "DD-MM-YYYY").day(3).format("DD/MM"));
                            $("#Jueves").text("J " + moment(value, "DD-MM-YYYY").day(4).format("DD/MM"));
                            $("#Viernes").text("V " + moment(value, "DD-MM-YYYY").day(5).format("DD/MM"));
                            $("#Sabado").text("S " + moment(value, "DD-MM-YYYY").day(6).format("DD/MM"));
                            $("#Domingo").text("D " + moment(value, "DD-MM-YYYY").day(7).format("DD/MM"));
                            getWorkDaysForWeek(firstDate);
                        }
                        jQuery("#weeklyDatePicker").val(firstDate + " - " + lastDate);
                        $("#addToTable").removeAttr('disabled');
                    }

                    BlockUIManager.unblock("#wrapper");

                }

            });
            var d = new Date();
            var strDate = d.getDate() + "-" + (d.getMonth() + 1) + "-" + d.getFullYear();
            $("#weeklyDatePicker").val = strDate;

            firstDate = moment(strDate, "DD-MM-YYYY").day(1).format("DD-MM-YYYY");
            lastDate = moment(strDate, "DD-MM-YYYY").day(7).format("DD-MM-YYYY");

            $("#Lunes").text("L " + moment(strDate, "DD-MM-YYYY").day(1).format("DD/MM"));
            $("#Martes").text("M " + moment(strDate, "DD-MM-YYYY").day(2).format("DD/MM"));
            $("#Miercoles").text("M " + moment(strDate, "DD-MM-YYYY").day(3).format("DD/MM"));
            $("#Jueves").text("J " + moment(strDate, "DD-MM-YYYY").day(4).format("DD/MM"));
            $("#Viernes").text("V " + moment(strDate, "DD-MM-YYYY").day(5).format("DD/MM"));
            $("#Sabado").text("S " + moment(strDate, "DD-MM-YYYY").day(6).format("DD/MM"));
            $("#Domingo").text("D " + moment(strDate, "DD-MM-YYYY").day(7).format("DD/MM"));
            jQuery("#weeklyDatePicker").val(firstDate + " - " + lastDate);
            getWorkDaysForWeek(firstDate)
            $("#addToTable").removeAttr('disabled');

        });
function getWorkDaysForWeek(firstDate) {
      BlockUIManager.block("#wrapper"); 
    api = $("#datatable-editable").DataTable().clear().draw();
    api.clear();

    $.ajax({
        contentType: 'application/json; charset=utf-8',
        url: "GetWeekByUser",
        data: JSON.stringify({ firstDate: firstDate }),
        type: "POST",
        success: function (result) {
            $.each(result, function (key, V) {
                EditableTable.rowAddWitchData(V);
            });
        }
    }).done(function () {
        api.draw();
        BlockUIManager.unblock("#wrapper");
    });
}