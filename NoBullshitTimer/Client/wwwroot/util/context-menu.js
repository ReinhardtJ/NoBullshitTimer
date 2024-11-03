function getBoundingClientRect(element) {
    return element.getBoundingClientRect();
}


function getWindowDimensions() {
    return {
        width: window.innerWidth,
        height: window.innerHeight
    };
}


function setElementPosition(element, top, left) {
    element.style.top = top + 'px';
    element.style.left = left + 'px';
}

window.getBoundingClientRect = getBoundingClientRect;
window.getWindowDimensions = getWindowDimensions;
window.setElementPosition = setElementPosition;
