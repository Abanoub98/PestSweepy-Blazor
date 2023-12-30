//export function generateAndDownloadPdf(filename) {
//    const doc = new jspdf.jsPDF({
//        orientation: 'p',
//        unit: 'pt',
//        format: 'a4'
//    });

//    return new Promise((resolve, reject) => {
//        doc.html($('#Report')[0], {
//            html2canvas: {
//                // insert html2canvas options here, e.g.
//                scale: 0.65
//            }
//        }).then(() => doc.save(filename));;
//    });
//}

window.DownloadPdf = function () {
	var printContents = document.getElementById('Report').innerHTML;
	var originalContents = document.body.innerHTML;

	document.body.innerHTML = printContents;

	window.print();

	document.body.innerHTML = originalContents;
};