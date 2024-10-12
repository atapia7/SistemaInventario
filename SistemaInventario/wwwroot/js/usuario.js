let datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#tblDatos').DataTable({
        "initComplete": function () {
            // Agregar clase de margen de Bootstrap al buscador
            $('.dataTables_filter').addClass('mb-2');
        },
        "language": {
            "lengthMenu": "Mostrar _MENU_ Registros Por Pagina",
            "zeroRecords": "Ningun Registro",
            "info": "Mostrar page _PAGE_ de _PAGES_",
            "infoEmpty": "no hay registros",
            "infoFiltered": "(filtered from _MAX_ total registros)",
            "search": "Buscar",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            },
        },
        "ajax": {
            "url": "/Admin/Usuario/ObtenerTodos"
        },
        "columns": [
            { "data": "email", },
            { "data": "nombres", },
            { "data": "apellidos",},
            { "data": "phoneNumber",},
            { "data": "role", },
            {
                "data": { id:"id",lockoutEnd:"lockoutEnd"},
                "render": function (data) {
                    let today = new Date().getTime();
                    let bloqueo = new Date(data.lockoutEnd).getTime();
                    if (bloqueo > today) {
                        return `
                        <div class="text-center">
                           <a onclick=BloquearDesbloquear('${data.id}') class="btn btn-danger text-white" style="cursor:pointer">
                                <i class="bi bi-unlock-fill"></i>Desbloquear
                           </a> </div>`;
                    } else {
                        return `
                        <div class="text-center">
                           <a onclick=BloquearDesbloquear('${data.id}') class="btn btn-success text-white" style="cursor:pointer">
                                <i class="bi bi-lock-fill"></i>Bloquear
                           </a> </div>`;
                    }
                    
                }, "width": "10%"
            }
        ]

    });
}

function BloquearDesbloquear(id) {

            $.ajax({
                type: "POST",
                url: '/Admin/Usuario/BloquearDesbloquear',
                data: JSON.stringify(id),
                contentType: "application/json",
                success: function (data) {
                    if (data.success) {
                        notificar(data.message,'success');
                        datatable.ajax.reload();
                    }
                    else {
                        notificar(data.message, 'warning');
                    }
                }
    });
}