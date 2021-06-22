import Map from 'ol/Map';
import View from 'ol/View';
import TileLayer from 'ol/layer/Tile';
import VectorLayer from 'ol/layer/Vector';
import TileSource from 'ol/source/TileWMS';
import VectorSource from 'ol/source/Vector';
import ImageWMS from 'ol/source/ImageWMS';
import ImageLayer from 'ol/layer/Image';
import OSM from 'ol/source/OSM';
import WKT from 'ol/format/WKT';
import { OlDefault } from './OlDefault';
import { XYZ, BingMaps } from 'ol/source';
import { METERS_PER_UNIT } from 'ol/proj';
import { Draw } from 'ol/interaction';
import { transform } from 'ol/proj';
import Point from 'ol/geom/Point';
import Feature from 'ol/Feature';

let map;

let mapStateForwardList = [];

let mapStatePreviousList = [];

let mapShouldUpdate = false;

const getMap = () => map;

const createMap = (options) => {

    const opts = { ...OlDefault.olMap, ...OlDefault.olOptions, ...options };

    map = new Map({
        pixelRatio: 1,
        view: createView(opts.view),
        layers: opts.layers,
        target: 'map',
        controls: [] // default zoom butonlarini/kontrollerini gizlemek icin kullanilmis olup, butonlarin aktif edilmesi icin bu degeri yorum satiri haline getiriniz
    });

    return map;
}

const addFeature = (layer, feature) => {

    const source = getLayerSource(layer);

    source.addFeature(feature);
}

const createView = (options) => {

    let opts = { ...OlDefault.olView, ...options };

    return new View({
        center: opts.center,
        projection: opts.projection,
        rotation: opts.rotation,
        maxZoom: 18,
        zoom: opts.zoom
    });
}

const createXyzSource = (options) => {

    let opts = { ...OlDefault.olXyzSource, ...options };

    return new XYZ({
        url: opts.url,
        crossOrigin: opts.crossOrigin
    });
}

const createBingSource = (options) => {

    let opts = { ...OlDefault.olBingSource, ...options };

    return new BingMaps({
        crossOrigin: opts.crossOrigin,
        key: opts.key,
        imagerySet: opts.imagerySet
    });
}

const createOsmSource = (options) => {

    let opts = { ...OlDefault.olOsmSource, ...options };

    return new OSM({
        url: opts.url,
        crossOrigin: opts.crossOrigin
    });
}

const addLayer = (layer) => map.addLayer(layer);

const addLayers = (layers) => {

    for (let i = 0; i < layers.length; i++) {

        addLayer(layers[i]);
    }
}

const mapBack = () => {

    if (mapStatePreviousList.length <= 0) {

        return;
    }

    let mapState = mapStatePreviousList.pop();

    if (!mapState) {

        return;
    }

    let state = {
        scaleValue: getScale(),
        centerPoint: getCenterPoint(),
        rotationDegree: getRotationDegree({})
    };

    if (mapState.scaleValue === state.scaleValue &&
        mapState.centerPoint === state.centerPoint &&
        mapState.rotationDegree === state.rotationDegree) {

        mapState = mapStatePreviousList.pop();

        if (!mapState) {

            return;
        }
    }

    mapStateForwardList.push(mapState);

    setCenterPoint({ centerPoint: mapState.centerPoint });

    setRotationDegree({ rotationDegree: mapState.rotationDegree });

    setScale({ scaleValue: mapState.scaleValue });

    mapShouldUpdate = false;
}

const mapForward = () => {

    if (mapStateForwardList.length <= 0) {

        return;
    }

    let mapState = mapStateForwardList.pop()

    if (!mapState) {

        return;
    }

    mapStatePreviousList.push(mapState);

    setCenterPoint({ centerPoint: mapState.centerPoint });

    setRotationDegree({ rotationDegree: mapState.rotationDegree });

    setScale({ scaleValue: mapState.scaleValue });

    mapShouldUpdate = false;
}

const updatePermalink = () => {

    if (!mapShouldUpdate) {

        mapShouldUpdate = true;

        return;
    }

    let state = {
        scaleValue: getScale(),
        centerPoint: getCenterPoint(),
        rotationDegree: getRotationDegree({})
    };

    mapStatePreviousList.push(state);

    mapStateForwardList = [];
}

const getProjection = () => getView().getProjection();

const getScale = () =>
    getResolution() * METERS_PER_UNIT[getProjection().getUnits()] * 39.37 * (25.4 / 0.28);

const setScale = (options) => {

    let opts = { ...OlDefault.olScale, ...options };

    setResolution({
        resolutionValue:
            (opts.scaleValue / (METERS_PER_UNIT[getProjection().getUnits()] * 39.37 * (25.4 / 0.28)))
    });
}

const getResolution = () => getView().getResolution();

const setResolution = (options) => {

    let opts = { ...OlDefault.olResolution, ...options };

    getView().setResolution(opts.resolutionValue);
}

const getView = () => map.getView();

const getRotationDegree = () => getView().getRotation();

const setRotationDegree = (options) => {

    let opts = { ...OlDefault.olRotation, ...options };

    getView().setRotation(opts.rotationDegree);
}

const getCenterPoint = () => getView().getCenter();

const setCenterPoint = (options) => {

    let opts = { ...OlDefault.olCenter, ...options };

    getView().setCenter(opts.centerPoint);
}

const resetMap = (layerName) => {

    if (layerName) {

        const layer = findLayer(layerName);

        const feature = layer.getSource().getFeatures()[0];

        viewLayerExtent(layerName, feature);

    } else {

        const { view } = OlDefault.olOptions;

        setCenterPoint({ centerPoint: view.center });

        setZoomLevel({ zoomLevel: view.zoom });
    }
}

const setZoomLevel = (options) => {

    let opts = { ...OlDefault.olZoom, ...options };

    getView().setZoom(opts.zoomLevel);
}

const addInteraction = (interaction) => map.addInteraction(interaction);

const createDrawInteraction = (name, layer, type) => {

    const source = getLayerSource(layer);

    const interaction = new Draw({
        type: type.toString(),
        source: source,
    });

    interaction.set("name", name);

    interaction.setActive(false);

    return interaction;
}

const getInteraction = (name) => {

    let interaction = map.getInteractions().
        getArray().
        find(x => x.get("name") === name);

    return interaction;
}

const getActiveInteraction = () => {

    let interaction = map.
        getInteractions().
        getArray().
        find(x => x.get("name") && x.get("active"))

    return interaction;
}

const enableInteraction = (name) => {

    const interaction = getInteraction(name);

    interaction.setActive(true)
}

const disableInteraction = (name) => {

    const interaction = getInteraction(name);

    interaction.setActive(false)
}

const getZoom = () => getView().getZoom();

const shortCut = (event) => {

    let charCode = (event.which) ? event.which : event.keyCode;

    if (charCode === 27) {

        pan();
    }
}

const pan = () => {

    let interaction = getActiveInteraction();

    if (interaction) {

        interaction.setActive(false);
    }
}

const updateLayerSource = (name, params) => getLayerSource(name).updateParams(params);

const getLayerSource = (name) => findLayer(name).getSource();

const getLayerByThematic = () => findLayer('thematiclayer');

const findLayer = (name) => map.getLayers().getArray().find(x => x.get("name") === name)

const createTileLayerSource = (options) => {

    const opts = { ...OlDefault.olTileWmsSource, ...options };

    return new TileSource({
        urls: opts.urls,
        crossOrigin: opts.crossOrigin,
        params: opts.params,
        format: opts.format,
        width: 256,
        height: 256
    });
}

const createVectorLayerSource = (options) => {

    const opts = { ...OlDefault.olVectorSource, ...options };

    return new VectorSource({
        format: opts.format,
        loader: opts.loader,
        projection: opts.projection,
        features: opts.feature
    });
}

const createImageSource = (options) => {

    const opts = { ...OlDefault.olTileWmsSource, ...options };

    return new ImageWMS({
        url: opts.url,
        crossOrigin: opts.crossOrigin,
        params: opts.params,
        serverType: 'geoserver',
        imageLoadFunction: opts.imageLoadFunction
    });
}

const createImageLayer = (options) => {
    const opts = { ...OlDefault.olTileWmsSource, ...options };

    return new ImageLayer({
        source: opts.source,
        name: opts.name,
        zIndex: 999
    });
}

const createTileLayer = (options) => {

    const opts = { ...OlDefault.olTileLayer, ...options };

    return new TileLayer({
        source: opts.source,
        name: opts.name,
        visible: opts.visible
    });
}

const createVectorLayer = (options) => {

    const opts = { ...OlDefault.olVectorLayer, ...options };

    return new VectorLayer({
        opacity: opts.opacity,
        source: opts.source,
        name: opts.name,
        visible: opts.visible
    });
}

const transformProjection = (feature, sourceProjection, destinationProjection) => {

    if (!destinationProjection) {

        destinationProjection = OlDefault.olOptions.view.projection
    }

    return transform(feature.getCoordinates(), sourceProjection, destinationProjection);
}

const convertToFeature = (wkt, wktProjection, featureProjection) => {

    if (!featureProjection) {

        featureProjection = OlDefault.olOptions.view.projection
    }

    return new WKT().readFeature(wkt, {
        dataProjection: wktProjection,
        featureProjection: featureProjection
    });
}

const convertToWkt = (feature, wktProjection, featureProjection) => {

    if (!featureProjection) {

        featureProjection = OlDefault.olOptions.view.projection
    }

    return new WKT().writeGeometry(feature.getGeometry(), {
        dataProjection: wktProjection,
        featureProjection: featureProjection
    });
}

const clearMap = () => {

    const vectorLayers = map.getLayers().
        getArray().
        filter(x => x instanceof VectorLayer && x.get('name') !== 'regionlayer');

    clearLayers(vectorLayers);
}

const clearLayersByName = (names) => {

    for (const name of names) {

        getLayerSource(name).clear();
    }
}

const clearLayers = (layers) => {

    for (const layer of layers) {

        layer.getSource().clear();
    }
}

const clearLayerByThematic = () => {

    let thematicLayerSource = findLayer('thematiclayer').getSource();

    thematicLayerSource.params_ = {};

    thematicLayerSource.refresh();

    let timer = setTimeout(() => { thematicLayerSource.url_ = ''; }, 1000);

    clearTimeout(timer);
}

const refreshLayer = (name) => {

    const source = getLayerSource(name);

    const params = source.getParams();

    params.t = new Date().getMilliseconds();

    source.updateParams(params);
}

const refreshWmsLayer = () => refreshLayer("wms_layer");

const viewLayerExtent = (layerName, feature, zoomLevel) => {

    let layer = findLayer(layerName);

    if (layer.getSource().getFeatures().length > 0) {

        if (!zoomLevel) {

            getView().fit(feature.getGeometry(), { padding: [170, 50, 30, 150], minResolution: 50 });
        }
        else {

            getView().fit(feature.getGeometry(), { zoomLevel: zoomLevel });
        }
    }
}

const getMapExtent = (options, onComplate) => {

    let extent = getView().calculateExtent(getMap().getSize());

    if (onComplate) {

        onComplate(extent);
    }
    else {
        return extent;
    }
}

const createPoint = function (options) {

    let point = new Point(options.coordinate);

    return new Feature({
        name: 'Point',
        geometry: point
    });
};

const zoomToWkt = (wkt, zoomLevel, projection, layer) => {

    const feature = convertToFeature(wkt, projection);

    zoomToFeature(feature, zoomLevel, layer)
}

const zoomToFeature = (feature, zoomLevel, layer = "draw_layer") => {

    addFeature(layer, feature);

    viewLayerExtent(layer, feature, zoomLevel);
}

export const mapTool = {
    refreshWmsLayer,
    createMap,
    getMap,
    createView,
    createXyzSource,
    createBingSource,
    createOsmSource,
    addLayer,
    addLayers,
    mapBack,
    mapForward,
    updatePermalink,
    getProjection,
    getScale,
    setScale,
    getResolution,
    setResolution,
    getView,
    getRotationDegree,
    setRotationDegree,
    getCenterPoint,
    setCenterPoint,
    resetMap,
    setZoomLevel,
    addInteraction,
    createDrawInteraction,
    getInteraction,
    getActiveInteraction,
    enableInteraction,
    disableInteraction,
    getZoom,
    updateLayerSource,
    getLayerSource,
    getLayerByThematic,
    findLayer,
    createTileLayerSource,
    createVectorLayerSource,
    createTileLayer,
    createVectorLayer,
    transformProjection,
    convertToFeature,
    convertToWkt,
    pan,
    addFeature,
    clearLayers,
    clearMap,
    clearLayersByName,
    refreshLayer,
    viewLayerExtent,
    createImageSource,
    getMapExtent,
    createImageLayer,
    clearLayerByThematic,
    createPoint,
    shortCut,
    zoomToWkt,
    zoomToFeature
}

export default mapTool;

