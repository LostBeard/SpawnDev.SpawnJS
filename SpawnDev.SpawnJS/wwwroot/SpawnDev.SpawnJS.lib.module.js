/* By Todd Tanner aka github.com/LostBeard 2026 */

(function () {
    if (globalThis.SpawnJSInterop) return;

    class SpawnJSInterop {
        static instances = {};
        static _idNext = 0;
        verbose = true;
        id = '';
        dotnetRuntime = null;
        constructor(dotnetRuntime) {
            this.dotnetRuntime = dotnetRuntime;
            //this.username = username;
            this.id = `_${++SpawnJSInterop._idNext}`;
            SpawnJSInterop.instances[this.id] = this;
            if (this.verbose) console.log('new SpawnJSInterop', this.id, dotnetRuntime);
        }
        _in(key, obj) {
            if (obj === null || obj === void 0) return false;
            try {
                return key in Object(obj);
            } catch { }
            return false;
        }
        pathObjectInfo(rootObject, path) {
            if (rootObject === null || rootObject === void 0) {
                // callers must call with the globalThis if they wish to use it as the rootObject.
                throw new DOMException('spawnJSInterop.pathObjectInfo error: rootObject cannot be null');
            }
            var parent = rootObject;
            var target;
            var propertyName;
            var shortCircuit = false;
            if (typeof path === 'string' && !(this._in(path, parent))) {
                var parts = path.split('.');
                propertyName = parts[parts.length - 1];
                var part;
                for (var i = 0; i < parts.length - 1; i++) {
                    part = parts[i];
                    if (part[part.length - 1] === '?') {
                        // ? null conditonal found
                        // if parent does not exist allow undefined/null parent instead of throwing exception
                        part = part.substring(0, part.length - 1);
                        parent = parent[part];
                        if (parent === void 0 || parent === null) {
                            shortCircuit = true;
                            break;
                        }
                    }
                    else {
                        parent = parent[part];
                    }
                }
                if (!shortCircuit) {
                    target = parent[propertyName];
                }
            }
            else {
                propertyName = path;
                target = parent[propertyName];
            }
            return {
                shortCircuit,   // bool - true if the pathfinding short circuited due to a null-conditional
                parent,         // any - only null or undefined if short circuited due to a null-conditional
                propertyName,   // any
                target,         // any
            };
        }
        hasOwnPropertySafe(obj, key) {
            if (obj === null || obj === undefined) return false;
            try {
                return typeof obj === 'object' ? obj.hasOwnProperty(key) : Object(obj).hasOwnProperty(key);
            } catch { }
            return false;
        }
        setProperty(/* object */ target, /* any */ identifier, /* any */ value) {
            var pathInfo = this.pathObjectInfo(target, identifier);
            if (pathInfo.shortCircuit) return;
            pathInfo.parent[pathInfo.propertyName] = value;
        }
        deleteProperty(/* object */ target, /* any */ identifier) {
            var pathInfo = this.pathObjectInfo(target, identifier);
            if (pathInfo.shortCircuit) return undefined;
            delete pathInfo.parent[pathInfo.propertyName];
        }
        getProperty(/* object */ target, /* any */ identifier) {
            var pathInfo = this.pathObjectInfo(target, identifier);
            if (pathInfo.shortCircuit) return undefined;
            if (typeof pathInfo.target === 'function') {
                return pathInfo.target.bind(pathInfo.parent);
            }
            return pathInfo.target;
        }
        invokeProperty(/* object */ target, /* any */ identifier, /* any */ args) {
            var pathInfo = this.pathObjectInfo(target, identifier);
            if (pathInfo.shortCircuit) return undefined;
            return pathInfo.target.apply(pathInfo.parent, args);
        }
        invokePropertyConstructor(/* object */ target, /* any */ identifier, /* any */ args) {
            var pathInfo = this.pathObjectInfo(target, identifier);
            if (pathInfo.shortCircuit) return undefined;
            return new pathInfo.target(...args);
        }
        newEasyPromise() {
            var _resolve = null;
            var _reject = null;
            var promise = new Promise((resolve, reject) => {
                _resolve = resolve;
                _reject = reject;
            });
            promise.resolve = _resolve;
            promise.reject = _reject;
            return promise
        }
        copyProperty(srcObj, srcKey, destObj, destKey) {
            destObj[destKey] = srcObj[srcKey];
        }
        moveProperty(srcObj, srcKey, destObj, destKey) {
            destObj[destKey] = srcObj[srcKey];
            delete destObj[destKey];
        }
        returnMe(/* any */ value) {
            return value;
        }
        // Creates a new function for .Net to use wit hJS as it needs to allow JS to call into .Net
        registerCallback(id) {
            return (...args) => {
                var ret = this._JSToNetCall(id, args);
                return ret == null || ret.length == 0 ? null : ret[0];
            }
        }
        // Creates a new function for .Net to use wit hJS as it needs to allow JS to call into .Net
        registerCallbackVoid(id) {
            return (...args) => {
                this._JSToNetCall(id, args);
            }
        }
        _JSToNetCall() {
            // this method is a placeHolder and will be overwritten
            // by SpawnJSRuntime immediately after constructed by its own JSExport-ed method.
            // this placeholder is here for clarity.
        }
        jsToNetCall(/* string */ cmd, ...args) {
        // intentionally extra code here to allow breakpoint return value debugging
            var ret = this._JSToNetCall(cmd, args);
            return ret;
        }
        jsToNetCallApply(/* string */ cmd, /* Array */ args) {
            // intentionally extra code here to allow breakpoint return value debugging
            var ret = this._JSToNetCall(cmd, args);
            return ret;
        }
        _netToJSCall(/* string */ cmd, /* Array */ argArray) {
            if (this.verbose) console.log(">> _netToJSCall::", cmd, argArray);
            var ret = this[cmd](...argArray);
            if (this.verbose) console.log("<< _netToJSCall::", ret);
            // package the results in an array so it can be read as a JSObject and then pulled into as needed wit hthe correct typing and marshalling
            return [ret];
        }
        async _netToJSCallAsync(/* string */ cmd, /* Array */ argArray) {
            if (this.verbose) console.log(">> _netToJSCallAsync::", cmd, argArray);
            var ret = await this[cmd](...argArray);
            if (this.verbose) console.log("<< _netToJSCallAsync::", ret);
            // package the results in an array so it can be read as a JSObject and then pulled into as needed wit hthe correct typing and marshalling
            return [ret];
        }
    }
    globalThis.SpawnJSInterop = SpawnJSInterop;
})()