// Download CSV file
function downloadCsv(fileName, data) {
    const link = document.createElement("a");
    link.download = fileName;
    link.href = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + data;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};

// Scroll to top
let btn = document.getElementById("myBtn");

btn.addEventListener('click', () => {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
});