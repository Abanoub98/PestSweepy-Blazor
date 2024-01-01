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
//                scale: 0.6,
//                y: 0,
//                scrollY: 0
//            },
//            pagebreak: { mode: 'avoid-all', before: '#page2el' }
//        }).then(() => doc.save(filename));;
//    });
//}

//window.DownloadPdf = function () {
//	var printContents = document.getElementById('Report').innerHTML;
//	var originalContents = document.body.innerHTML;

//	document.body.innerHTML = printContents;

//	window.print();

//	document.body.innerHTML = originalContents;
//};

window.DownloadPdf = function (filename) {
    const element = document.getElementById("Report");

    var opt =
    {
        margin: [0.5, -0.1, 0, 0],
        filename: filename,
        image: { type: 'jpeg', quality: 0.98 },
        html2canvas: { scale: 3, y: 0, scrollY: 0 },
        jsPDF: { unit: 'in', format: 'letter', orientation: 'portrait' },
        pagebreak: { mode: 'avoid-all', before: '#page2el' }
    };

    // New Promise-based usage:
    html2pdf()
        .set(opt)
        .from(element)
        .save();
};