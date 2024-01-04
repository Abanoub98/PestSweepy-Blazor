

window.DownloadPdf = function () {
	var printContents = document.getElementById('Report').innerHTML;
	document.body.innerHTML = printContents;

	window.print();
	window.location.reload();
	/*window.print();*/
};

//window.DownloadPdf = function (filename) {
//    const element = document.getElementById("Report");

//    var opt =
//    {
//        margin: [0.5, -0.1, 0, 0],
//        filename: filename,
//        image: { type: 'jpeg', quality: 0.98 },
//        html2canvas: { scale: 3, y: 0, scrollY: 0 },
//        jsPDF: { unit: 'in', format: 'letter', orientation: 'portrait' },
//        pagebreak: { mode: 'avoid-all', before: '#page2el' }
//    };

//    // New Promise-based usage:
//    html2pdf()
//        .set(opt)
//        .from(element)
//        .save();
//};