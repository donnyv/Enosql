/// <reference path="util.js" />

// Globals
// e.g. [{ collectionNameUp: "USERS", collectionName: "Users", fileName: "Users.json", _id:<ObjectId> }]
var _system_namespaces = [];

var _collections = {};
var _collectionsSysIds = {};

var ok = "ok";

function CreateCollection(name) {
    if (CollectionExists(name))
        throw "Collection name already exists!"

    var ns = {
        _id: new ObjectId().toString(),
        collectionName: name,
        collectionNameUp: name.toUpperCase(),
        fileName: name.toLowerCase() + ".json"
    };
    _system_namespaces.push(ns);
    _collections[ns._id] = [];
    _collectionsSysIds[ns._id] = {};

    return ok;
}

function CollectionExists(name) {
    var col = _GetNamespace(name);
    return col ? true : false;
}

function DropCollection(name) {
    var nsInfo = _GetNamespaceIfExists(name);
    delete _collections[nsInfo._id];
    delete _collectionsSysIds[nsInfo._id];
    _RemoveNamespace(nsInfo._id);
    return ok;
}

function Insert(collectionName, item) {
    item = JSON.parse(item);
    var col = _GetNamespaceIfExists(collectionName);

    if (typeof item._id !== "undefined") {
        if (!_IsUnique(col._id, item._id))
            return "_id already exists";
    }

    if (typeof item._id === "undefined")
        item._id = new ObjectId().toString();
    
    if (item._id == null || item._id.trim() == "")
        item._id = new ObjectId().toString();

    _collections[col._id].push(item);
    _collectionsSysIds[col._id][item._id] = {};

    return ok;
}

function FindById(collectionName, id) {
    var nsInfo = _GetNamespaceIfExists(collectionName);
    var col = _collections[nsInfo._id]
    for (var i = 0, l = col.length; i < l; i++) {
        if (col[i]._id == id) {
            return JSON.stringify([col[i]]);
        }
    }
    return "[]";
}

function FindAll(collectionName) {
    var col = _GetNamespaceIfExists(collectionName);
    return JSON.stringify(_collections[col._id]);
}

function RemoveAll(collectionName) {
    var col = _GetNamespaceIfExists(collectionName);
    _collections[col._id] = [];
    return ok;
}

function Remove(collectionName, id) {
    var nsInfo = _GetNamespaceIfExists(collectionName);
    var col = _collections[nsInfo._id]
    for (var i = 0, l = col.length; i < l; i++) {
        if (col[i]._id == id) {
            col.splice(i, 1);
            delete _collectionsSysIds[col._id][id];
            return;
        }
    }

    return -1
}

function Save(collectionName, item) {
    item = JSON.parse(item);
    var nsInfo = _GetNamespaceIfExists(collectionName);
    var col = _collections[nsInfo._id]

    // if id doesn't exist then do an insert
    if (typeof item._id === "undefined" || item._id == null || item._id.trim() == "") {
        Insert(collectionName, item);
        return ok;
    }

    // if id exist and has value then find and update item
    for (var i = 0, l = col.length; i < l; i++) {
        if (col[i]._id == item._id) {
            col[i] = item;
            return ok;
        }
    }

    return -1
}

function AddNamespaces(ns) {
    _system_namespaces = JSON.parse(ns);
    return ok
}

function GetNamespaces() {
    return JSON.stringify(_system_namespaces);
}

function GetNamespace(name) {
    return JSON.stringify(_GetNamespace(name));
}

function BuildCollectionIds() {
    var colns;
    for (var i = 0, l = _system_namespaces.length; i < l; i++) {
        colns = _system_namespaces[i];
        _collectionsSysIds[colns._id] = {};
        for (var j = 0, l2 = _collections[colns._id].length; j < l2; j++) {
            _collectionsSysIds[colns._id][_collections[colns._id][j]._id] = {};
        }
    }
    return ok;
}

function GetCollectionSysIds() {
    return JSON.stringify(_collectionsIndex);
}

function InitialCollectionLoad(cols) {
    cols = JSON.parse(cols);
    for (var i = 0, l = cols.length; i < l; i++) {
        _collections[cols[i].id] = cols[i].data;
    }
    return ok;
}

// Internal methods
function _PropertyExist(name, obj) {
    var _propname = name.toUpperCase();
    for (var prop in obj) {
        if (obj.hasOwnProperty(prop)) {
            if (prop.toUpperCase() == _propname) {
                return true;
            }
        }
    }
    return false;
}

function _GetNamespace(name) {
    for (var i = 0, l = _system_namespaces.length; i < l; i++) {
        if (_system_namespaces[i].collectionNameUp === name.toUpperCase())
            return _system_namespaces[i];
    }
    return null;
}

function _RemoveNamespace(id) {
    for (var i = 0, l = _system_namespaces.length; i < l; i++) {
        if (_system_namespaces[i]._id === id) {
            _system_namespaces.splice(i, 1);
            return ok;
        }
    }
    return -1;
}

function _GetNamespaceIfExists(name) {
    var col = _GetNamespace(name);
    if (!col)
        throw "Collection doesn't exist!";

    return col;
}

function _IsUnique(colid, id) {
    return id in _collectionsSysIds[colid] ? false : true;
}