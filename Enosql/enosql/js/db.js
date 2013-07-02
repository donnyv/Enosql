/// <reference path="util.js" />

// e.g. [{ collectionNameUp: "USERS", collectionName: "Users", fileName: "Users.json", _id:<ObjectId> }]
var _system_namespaces = [];

var _collections = {};

function _GetCollectionIfExists(name) {
    var col = GetCollection(name);
    if (!col)
        throw "Collection doesn't exist!";

    return col;
}

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
}

function CollectionExists(name) {
    var col = GetCollection(name);
    return col ? true : false;
}

function GetCollection(name) {
    for (var i = 0, l = _system_namespaces.length; i < l; i++) {
        if (_system_namespaces[i].collectionNameUp === name.toUpperCase())
            return _system_namespaces[i];
    }
    return null;
}

function PropertyExist(name, obj) {
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

function Insert(collectionName, item) {
    var col = _GetCollectionIfExists(collectionName);

    if(!PropertyExist("_id", item))
        item._id = new ObjectId().toString();

    _collections[col._id].push(item);
}

function FindById(collectionName, id) {
    var collInfo = _GetCollectionIfExists(collectionName);
    var col = _collections[collInfo._id]
    for (var i = 0, l = col.length; i < l; i++) {
        if (col[i]._id == id) {
            return JSON.stringify([col[i]]);
        }
    }
}

function FindAll(collectionName) {
    var col = _GetCollectionIfExists(collectionName);
    return JSON.stringify(_collections[col._id]);
}

function RemoveAll(collectionName) {
    var col = _GetCollectionIfExists(collectionName);
    _collections[col._id] = [];
}

function Remove(collectionName, id) {
    var collInfo = _GetCollectionIfExists(collectionName);
    var col = _collections[collInfo._id]
    for (var i = 0, l = col.length; i < l; i++) {
        if (col[i]._id == id) {
            col.splice(i, 1);
            return;
        }
    }

    return -1
}

function Update(collectionName, item) {
    var collInfo = _GetCollectionIfExists(collectionName);
    var col = _collections[collInfo._id]
    for (var i = 0, l = col.length; i < l; i++) {
        if (col[i]._id == item._id) {
            col[i] = item;
            return;
        }
    }

    return -1
}

function AddNamespaces(ns) {
    _system_namespaces = ns;
}

function GetNamespaces() {
    return JSON.stringify(_system_namespaces);
}

function InitialCollectionLoad(cols) {
    for (var i = 0, l = cols.length; i < l; i++) {
        _collections[cols[i].id] = cols[i].data;
    }
}