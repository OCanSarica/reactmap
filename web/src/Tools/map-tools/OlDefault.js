import KML from 'ol/format/KML';
import GeoJSON from 'ol/format/GeoJSON';
import GPX from 'ol/format/GPX';
import IGC from 'ol/format/IGC';
import TopoJSON from 'ol/format/TopoJSON';
import Projection from 'ol/proj/Projection';
import View from 'ol/View';
import { Fill, Stroke, Text, Circle } from 'ol/style';
import { defaults } from 'ol/control';
import { DragAndDrop } from 'ol/interaction';

const BING_KEY = 'cFtbdF0ydlkdvhXC5wKO~ybf3qUqF_cTjHHB99chFrg~Ahw3Fh7i7XxRxdYhYosI4GLxwKDsTBjC5J8xGXdFxVVxeg7fdlMR5D-rx8gM5e0i';

export class OlDefault {

    static olBaseLayer = {
        layerTitle: 'base_layer',
        layerName: '',
        baseMapName: '',
        functionName: ''
    };

    static olMapCanvas = {
        canvasId: 'map-canvas'
    };

    static olProjection = {
        code: 'EPSG:3857',
        units: undefined,
        extent: undefined
    };

    static olControl = {
        attribution: false,
        zoom: true,
        rotate: true
    };

    static olStyle = {
        image: new Circle(
            {
                fill: new Fill({ color: 'rgba(255,255,255,0.4)' }),
                stroke: new Stroke({ color: '#3399CC', width: 1.25 }),
                radius: 5
            }),
        fill: new Fill({ color: 'rgba(255,255,255,0.4)' }),
        stroke: new Stroke({ color: '#3399CC', width: 1.25 }),
        text: new Text({
            font: '12px Calibri,sans-serif',
            fill: new Fill({ color: '#000' }),
            stroke: new Stroke({ color: '#fff', width: 5 }),
            text: ''
        })
    };

    static olView = {
        projection: 'EPSG:3857',
        center: [0, 0],
        zoom: 1,
        rotation: 0
    };

    static olVectorSource = {
        format: undefined,
        loader: function () {
            return undefined;
        },
        projection: new Projection({ code: 'EPSG:3857', units: undefined, extent: undefined })
    };

    static olTileWmsSource = {
        urls: undefined,
        crossOrigin: 'Anonymous',
        params: { Layers: [] },
        format: 'image/png8',
        gutter: undefined,
        tileLoadFunction: undefined
    };

    static olXyzSource = {
        crossOrigin: 'Anonymous ',
        url: 'http://mts0.google.com/vt/lyrs=m&x={x}&y={y}&z={z}'
    };

    static olBingSource = {
        crossOrigin: 'Anonymous ',
        imagerySet: 'Road',
        key: BING_KEY
    };

    static olOsmSource = {
        crossOrigin: 'Anonymous ',
        url: 'http://{a-c}.tile.openstreetmap.fr/hot/{z}/{x}/{y}.png'
    };

    static olVectorLayer = {
        style: undefined,
        title: undefined,
        name: undefined,
        visible: true,
        opacity: 1
    };

    static olTileLayer = {
        title: '',
        source: undefined,
        opacity: 1,
        minResolution: undefined,
        maxResolution: undefined,
        visible: true
    };

    static olAddFeature = {
        wktString: '',
        dataProjection: 'EPSG:4326',
        featureProjection: 'EPSG:3857',
        layerTitle: '',
    };

    static olMap = {
        view: new View({ center: [0, 0], zoom: 4 }),
        layers: [],
        target: '',
        controls: defaults({ attribution: false, rotate: true, zoom: true }),
        interactions: new DragAndDrop({ formatConstructors: [GPX, GeoJSON, IGC, KML, TopoJSON] })
    };

    static olScale = {
        scaleValue: 50000
    };

    static olResolution = {
        resolutionValue: 0
    };

    static olZoom = {
        zoomLevel: 5
    };

    static olRotation = {
        rotationDegree: 0
    };

    static olCenter = {
        centerPoint: [0, 0]
    };

    static olOverlay = {
        overlay: undefined
    };

    static olMapEvent = {
        eventName: ''
    };

    static olOptions = {
        mapIdentifier: '#map-canvas',
        view: {
            center: [3925805.7727269167, 4678557.627279282],
            rotation: 0,
            projection: 'EPSG:3857',
            zoom: 7
        },
        controls: {
            attribution: false, rotate: true, zoom: true
        },
        target: {
            canvasId: 'map-canvas'
        }
    }
}

export default OlDefault;
