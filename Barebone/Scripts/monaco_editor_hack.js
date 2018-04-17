// idea : "vs/css" module defines CSSPlugin variable and function to override is CSSPlugin._CssLoader.prototype.createLinkTag
// How to properly access this variable???

// BrowserCSSLoader.prototype.createLinkTag = function (name, cssUrl, externalCallback, externalErrorback) {
//     var _this = this;
//     var linkNode = document.createElement('link');
//     linkNode.setAttribute('rel', 'stylesheet');
//     linkNode.setAttribute('type', 'text/css');
//     linkNode.setAttribute('data-name', name);
//     var callback = function () { return _this._onLoad(name, externalCallback); };
//     var errorback = function (err) { return _this._onLoadError(name, externalErrorback, err); };
//     this.attachListeners(name, linkNode, callback, errorback);

//     var embeddedResourceCssUrl = '/node_modules.monaco_editor.min.' + cssUrl.replace('/', '.').replace('-', '_');
//     console.log("rewrote url as: ", embeddedResourceCssUrl);

//     linkNode.setAttribute('href', embeddedResourceCssUrl);
//     return linkNode;
// };
