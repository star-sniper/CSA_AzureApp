window.imageUtilities = {
    validateImageDimensions: function (imageDataUrl, minWidth, minHeight, maxWidth, maxHeight) {
        return new Promise((resolve, reject) => {
            const img = new Image();
            img.src = imageDataUrl;
            img.onload = function () {
                if (
                    img.width >= minWidth &&
                    img.height >= minHeight &&
                    img.width <= maxWidth &&
                    img.height <= maxHeight
                ) {
                    resolve(true);
                } else {
                    resolve(false);
                }
            };
            img.onerror = function () {
                reject(false);
            };
        });
    },
};
