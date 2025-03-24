
function notificar(mensaje,tipo='info') {
    if (tipo && mensaje) {
        toastr[tipo](mensaje);
    }
}