var firstDate;
var lastDate;
var retCompanies;
var EditableTable;
var needChange = true;
var Lunes    =0;
var Martes   =0;
var Miercoles=0;
var Jueves   =0;
var Viernes  =0;
var Sabado   =0;
var Domingo = 0;
var cmbProyecto;
/**
* Theme: Montran Admin Template
* Author: Coderthemes
* Component: Editable
* 
*/

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
                aoColumns: [
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
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

            data = this.datatable.row.add(['', '', '', '0', '0', '0', '0', '0', '0', '0', actions]);
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
        },
        rowAddWitchData: function (V) {

            var actions,
                data,
                $row;

            actions = [
                '<a href="#" class="hidden on-editing save-row  btn btn-success waves-effect waves-light"><i class="md-save"></i></a>',
                '<a href="#" class="hidden on-editing cancel-row  btn btn-danger waves-effect waves-light"><i class=" md-clear"></i></a>',
                '<a href="#" class="on-default edit-row btn btn-primary waves-effect waves-light"><i class=" md-edit"></i></a>',
                '<a href="#" class="on-default remove-row  btn btn-danger waves-effect waves-light"><i class=" md-delete"></i></a>'
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
                .attr('data-companies', V.companies);

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
        },
        rowCancel: function ($row) {
            var _self = this,
                $actions,
                i,
                data;

            if ($row.hasClass('adding')) {
                this.rowRemove($row);
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

            $row.children('td').each(function (i) {
                var $this = $(this);

                if ($this.hasClass('actions')) {
                    _self.rowSetActionsEditing($row);
                }
                else if ($this.hasClass('cmbProyecto')) {

                    $.ajax({

                        url: "/proyects/getAllProyects", success: function (result) {
                            var htmlSelectEmpresa = '<select class="sselect2-container select2 form-control" id="cmbProyecto"  style="background: white;">';

                            $.each(result, function (key, value) {
                                htmlSelectEmpresa += '<option value="' + value.Id + '">' + value.Name + '</option>';
                            });
                            htmlSelectEmpresa += ' </select>';

                            $this.html(htmlSelectEmpresa);

                            $("#cmbProyecto").select2();
                            if ($this.data('proyectid') == undefined) {
                                $("#cmbProyecto").select2();
                            } else {
                                $("#cmbProyecto").select2().select2('val', $this.data('proyectid'));
                            }
                        }
                    });
                   
                    $(document.body).on("change", "#cmbProyecto", function () {
                        if ($this.is(":visible")) {
                            $.ajax({
                                url: "/proyects/getAllCompaniesForCompany",
                                data: { "id": $("#cmbProyecto").val() },
                                success: function (result) {

                                    var htmlSelectEmpresa = '<select class="js-example-basic-multiple" id="cmbEmpresa" style="background: white;" multiple="multiple">';
                                    $.each(result, function (key, value) {
                                        htmlSelectEmpresa += '<option value="' + value.Id + '">' + value.Name + '</option>';
                                    });
                                    htmlSelectEmpresa += ' </select>';
                                    $this.next().html(htmlSelectEmpresa);
                                    if (needChange) {
                                        needChange = false;
                                        var dataCompanies = $this.next().data('companies');
                                        if (dataCompanies != undefined) {
                                            if (!isNaN(dataCompanies)) {
                                                dataCompanies = UtilJs.toInt(dataCompanies);
                                            } else {
                                                dataCompanies = dataCompanies.split(",");
                                            }
                                            $("#cmbEmpresa").val(dataCompanies).trigger('change');
                                        }

                                    }

                                    $("#cmbEmpresa").select2();
                                }
                            });
                        }
                    });

                    _self.rowSetActionsEditing($row);

                }
                else if ($this.hasClass('cmbEmpresa')) {


                    var htmlSelectEmpresa = '<select class="js-example-basic-multiple" id="cmbEmpresa" name="states[]" style="background: white;" multiple="multiple">';

                    htmlSelectEmpresa += '<option value=""> </option>';

                    htmlSelectEmpresa += ' </select>';

                    $this.html(htmlSelectEmpresa);


                    $("#cmbEmpresa").select2();
                    needChange = true;

                    //$("#cmbEmpresa").val($this.data('companies')).trigger('change');
                }
                else if ($this.hasClass('cmbTipoTarea')) {

                    $.ajax({

                        url: "/proyects/getAllTypeTasks", success: function (result) {
                            var htmlSelectEmpresa = '<select class="sselect2-container select2 form-control" id="cmbTipoTarea"  style="background: white;">';

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

                else {
                    $this.html('<input type="number" min="0" max="24" class="form-control input-block" value="' + data[i] + '"/>');
                }
            });
            setTimeout(function () { $("#cmbProyecto").trigger("change"); }, 500);

        },

        rowSave: function ($row) {
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
                    retCompanies = [];
                    $.each($('#cmbEmpresa').select2('data'), function (i, val) {
                        retCompanies.push(val.id);
                    });
                    return {
                        currentCompanies: retCompanies,
                        lastCompanies: $this.data('companies')
                    };
                    return retCompanies;
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
                companies: values[1].currentCompanies,
                companiesOld: values[1].lastCompanies,
                TaskType:values[2].currentTaskType,
                TaskTypeOld: values[2].lastTaskType,
                Lunes:    Lunes,        
                Martes:   Martes,       
                Miercoles:Miercoles ,
                Jueves:   Jueves,      
                Viernes:  Viernes,      
                Sabado:   Sabado,      
                Domingo:  Domingo,     
                StartLoad: firstDate
            };
            
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                url: "SaveWeek",
                data: JSON.stringify(obj),
                type: "POST",
                success: function (result) {
             
                    if (result == "true") {
                        $.Notification.autoHideNotify('success', 'top right', 'Regristro Correcto', 'Se persitio crrectamente su carga de horas');
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

        rowRemove: function ($row) {
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
                    retCompanies = [];
                    $.each($('#cmbEmpresa').select2('data'), function (i, val) {
                        retCompanies.push(val.id);
                    });
                    return {
                        currentCompanies: retCompanies,
                        lastCompanies: $this.data('companies')
                    };
                    return retCompanies;
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
                companies: values[1].currentCompanies,
                companiesOld: values[1].lastCompanies,
                TaskType:values[2].currentTaskType,
                TaskTypeOld: values[2].lastTaskType,
                Lunes:    Lunes,        
                Martes:   Martes,       
                Miercoles:Miercoles ,
                Jueves:   Jueves,      
                Viernes:  Viernes,      
                Sabado:   Sabado,      
                Domingo:  Domingo,     
                StartLoad: firstDate
            };
            
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

    $("#weeklyDatePicker").datepicker({
        format: 'dd-mm-yyyy'

    });

    //Get the value of Start and End of Week
    addCommonListener({
        selector: "#weeklyDatePicker",
        name: "change",
        callBack: function () {

            var value = jQuery("#weeklyDatePicker").val();

            if (value.length == 10) {
                if (firstDate != moment(value, "DD-MM-YYYY").day(0).format("DD-MM-YYYY")) {
                 

                    
                    firstDate = moment(value, "DD-MM-YYYY").day(0).format("DD-MM-YYYY");
                    lastDate = moment(value, "DD-MM-YYYY").day(6).format("DD-MM-YYYY");
                    $("#Lunes").text("L " + moment(value, "DD-MM-YYYY").day(0).format("DD/MM"));
                    $("#Martes").text("M " + moment(value, "DD-MM-YYYY").day(1).format("DD/MM"));
                    $("#Miercoles").text("M " + moment(value, "DD-MM-YYYY").day(2).format("DD/MM"));
                    $("#Jueves").text("J " + moment(value, "DD-MM-YYYY").day(3).format("DD/MM"));
                    $("#Viernes").text("V " + moment(value, "DD-MM-YYYY").day(4).format("DD/MM"));
                    $("#Sabado").text("S " + moment(value, "DD-MM-YYYY").day(5).format("DD/MM"));
                    $("#Domingo").text("D " + moment(value, "DD-MM-YYYY").day(6).format("DD/MM"));
                    getWorkDaysForWeek(firstDate);

                }
                jQuery("#weeklyDatePicker").val(firstDate + " - " + lastDate);
                $("#addToTable").removeAttr('disabled');
            }

        }

    });



    var d = new Date();
    var strDate = d.getDate() + "-" + (d.getMonth() + 1) + "-" + d.getFullYear();
    $("#weeklyDatePicker").val = strDate;

    firstDate = moment(strDate, "DD-MM-YYYY").day(0).format("DD-MM-YYYY");
    lastDate = moment(strDate, "DD-MM-YYYY").day(6).format("DD-MM-YYYY");

    $("#Lunes").text("L " + moment(strDate, "DD-MM-YYYY").day(0).format("DD/MM"));
    $("#Martes").text("M " + moment(strDate, "DD-MM-YYYY").day(1).format("DD/MM"));
    $("#Miercoles").text("M " + moment(strDate, "DD-MM-YYYY").day(2).format("DD/MM"));
    $("#Jueves").text("J " + moment(strDate, "DD-MM-YYYY").day(3).format("DD/MM"));
    $("#Viernes").text("V " + moment(strDate, "DD-MM-YYYY").day(4).format("DD/MM"));
    $("#Sabado").text("S " + moment(strDate, "DD-MM-YYYY").day(5).format("DD/MM"));
    $("#Domingo").text("D " + moment(strDate, "DD-MM-YYYY").day(6).format("DD/MM"));
    jQuery("#weeklyDatePicker").val(firstDate + " - " + lastDate);
    getWorkDaysForWeek(firstDate)
    $("#addToTable").removeAttr('disabled');

});
function getWorkDaysForWeek(firstDate) {
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