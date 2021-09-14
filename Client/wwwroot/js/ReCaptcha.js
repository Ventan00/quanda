function isScriptLoaded() {
    const scripts = Array.from(document.getElementsByTagName('script'));
    return scripts.some(s => (s.src || '').startsWith('https://www.google.com/recaptcha/api.js'))
}

function render(dotNetObj, selector, sitekey) {
    var widgetId = grecaptcha.render(selector,
        {
            'sitekey': sitekey
        });

    dotNetObj.invokeMethodAsync('OnWidgetIdSet', widgetId);
}

export function tryRender(dotNetObj, selector, sitekey) {
    if (!isScriptLoaded()) {
        const script = document.createElement("script");
        script.src = '//www.google.com/recaptcha/api.js?render=explicit';
        script.async = true;
        script.defer = true;
        document.body.appendChild(script);
    }

    new Promise((resolve, reject) => {
        function isCaptchaApiLoaded() {
            if (typeof (grecaptcha) !== 'undefined' && typeof (grecaptcha.render) !== 'undefined')
                resolve();
            else
                setTimeout(() => isCaptchaApiLoaded(), 100);
        };

        isCaptchaApiLoaded();
    }).then(() => {
        render(dotNetObj, selector, sitekey);
    });
}

export function getResponse(widgetId) {
    return grecaptcha.getResponse(widgetId)
}

export function reset(widgetId) {
    grecaptcha.reset(widgetId)
}