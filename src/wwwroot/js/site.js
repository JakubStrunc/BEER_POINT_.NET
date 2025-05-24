(function () {
    // create the loading screen container
    const loadingScreen = document.createElement("div");
    loadingScreen.id = "loading-screen";
    loadingScreen.style.position = "fixed";
    loadingScreen.style.top = "0";
    loadingScreen.style.left = "0";
    loadingScreen.style.width = "100%";
    loadingScreen.style.height = "100%";
    loadingScreen.style.zIndex = "9999";
    loadingScreen.style.display = "flex";
    loadingScreen.style.justifyContent = "center";
    loadingScreen.style.alignItems = "center";

    // create the spinner wrapper
    const spinnerWrapper = document.createElement("div");
    spinnerWrapper.className = "spinner-content text-center";

    // create the spinner itself
    const spinner = document.createElement("div");
    spinner.className = "spinner-border text-dark";
    spinner.setAttribute("role", "status");
    spinner.style.width = "10rem"; // Set spinner size
    spinner.style.height = "10rem";

    // add hidden text inside the spinner for accessibility
    const hiddenText = document.createElement("span");
    hiddenText.className = "visually-hidden";
    hiddenText.textContent = "Loading...";

    spinner.appendChild(hiddenText);
    spinnerWrapper.appendChild(spinner);
    loadingScreen.appendChild(spinnerWrapper);
    document.body.appendChild(loadingScreen);

    // hide the spinner after the page has loaded
    window.addEventListener("load", function () {
        loadingScreen.style.opacity = "0"; 
        loadingScreen.style.transition = "opacity 0.5s ease-out";
        setTimeout(() => {
            loadingScreen.remove();
        }, 1000);
    });
})();
