// Vytvoří loading screen a vloží ho do DOMu
(function () {
    const loadingScreen = document.createElement("div");
    loadingScreen.id = "loading-screen";
    loadingScreen.style.position = "fixed";
    loadingScreen.style.top = "0";
    loadingScreen.style.left = "0";
    loadingScreen.style.width = "100%";
    loadingScreen.style.height = "100%";
    loadingScreen.style.backgroundImage = "url('~/img/backgroundbeeropoint.jpg')"; 
    loadingScreen.style.backgroundSize = "cover";
    loadingScreen.style.backgroundPosition = "center";
    loadingScreen.style.zIndex = "9999";
    loadingScreen.style.display = "flex";
    loadingScreen.style.justifyContent = "center";
    loadingScreen.style.alignItems = "center";

    const spinnerWrapper = document.createElement("div");
    spinnerWrapper.className = "spinner-content text-center";

    const spinner = document.createElement("div");
    spinner.className = "spinner-border text-dark";
    spinner.setAttribute("role", "status");
    spinner.style.width = "10rem";
    spinner.style.height = "10rem";

    const hiddenText = document.createElement("span");
    hiddenText.className = "visually-hidden";
    hiddenText.textContent = "Loading...";

    spinner.appendChild(hiddenText);
    spinnerWrapper.appendChild(spinner);
    loadingScreen.appendChild(spinnerWrapper);
    document.body.appendChild(loadingScreen);

    // Po načtení stránky skryj spinner
    window.addEventListener("load", function () {
        loadingScreen.style.opacity = "0";
        loadingScreen.style.transition = "opacity 0.5s ease-out";
        setTimeout(() => {
            loadingScreen.remove();
        }, 1000);
    });
})();
