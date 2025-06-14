function ShowConfirmationModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('ConfirmationModal')).show();
}
function HideConfirmationModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('ConfirmationModal')).hide();
}