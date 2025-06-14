window.showToastr = function (type, message) {
    console.log("showToastr called with:", type, message);
    switch (type) {
        case "success":
            toastr.success(message);
            break;
        case "error":
            toastr.error(message);
            break;
        case "warning":
            toastr.warning(message);
            break;
        case "info":
            toastr.info(message);
            break;
        default:
            console.warn("Invalid toastr type:", type);
            break;
    }
};