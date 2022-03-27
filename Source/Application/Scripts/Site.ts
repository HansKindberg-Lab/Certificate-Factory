import "bootstrap";
import "svgxuse";

//document.addEventListener("DOMContentLoaded", () => {
//	const automaticRedirectAttributeName = "data-automatic-redirect";
//	const automaticRedirectElement = document.querySelector("[" + automaticRedirectAttributeName + "]");

//	if (automaticRedirectElement) {
//		const automaticRedirectValue = automaticRedirectElement.getAttribute(automaticRedirectAttributeName);

//		if (automaticRedirectValue) {
//			const parts = automaticRedirectValue.split(";", 2);
//			const seconds = parseInt(parts[0]);
//			const url = parts[1];

//			console.log(`Redirecting to "${url}" in ${seconds} seconds.`);

//			setTimeout(() => {
//				window.location.href = url;
//			}, seconds * 1000);
//		}
//	}

//	const modalButton = document.querySelector("button[data-bs-toggle='modal']") as HTMLButtonElement;

//	if (modalButton) {
//		modalButton.click();
//		modalButton.remove();
//	}
//});