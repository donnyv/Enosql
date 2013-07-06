﻿/// <reference path="util.js" />

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

function Insert(collectionName, item) {
    var col = _GetCollectionIfExists(collectionName);

    if (typeof item._id !== "undefined") {
        if (!IsUnique(col._id, item._id))
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
    return ok;
}

function Remove(collectionName, id) {
    var collInfo = _GetCollectionIfExists(collectionName);
    var col = _collections[collInfo._id]
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
    var collInfo = _GetCollectionIfExists(collectionName);
    var col = _collections[collInfo._id]

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
    _system_namespaces = ns;
    return ok
}

function GetNamespaces() {
    return JSON.stringify(_system_namespaces);
}

function LoadCollectionSysIds(uids) {
    _collectionsIndex = uids;
}

function GetCollectionSysIds() {
    return JSON.stringify(_collectionsIndex);
}

function IsUnique(colid, id) {
    return id in _collectionsSysIds[colid] ? false : true;
}

function InitialCollectionLoad(cols) {
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

function _GetCollectionIfExists(name) {
    var col = GetCollection(name);
    if (!col)
        throw "Collection doesn't exist!";

    return col;
}