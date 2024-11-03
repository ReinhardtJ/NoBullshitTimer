window.playSound = function (soundFile) {
    const audio = new Audio(soundFile);
    void audio.play();
}
