window.saveAsFile = function (fileName, byteBase64String) {
    const link = document.createElement('a');
    link.href = "data:application/octet-stream;base64," + byteBase64String;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};