// install-prompt.js

let deferredPrompt;

window.addEventListener('beforeinstallprompt', (event) => {
    event.preventDefault();
    deferredPrompt = event;
/*    startDownloadButton.style.display = 'block';*/
});

// Initialize deferredPrompt in case the beforeinstallprompt event is not fired.
window.addEventListener('load', () => {
    deferredPrompt = null;
});

window.Download = async function () {
    if (deferredPrompt !== null) {
        deferredPrompt.prompt();
        const { outcome } = await deferredPrompt.userChoice;
        if (outcome === 'accepted') {
            deferredPrompt = null;
        }
    } else {
        alert("App Already Installed");
    }
};

