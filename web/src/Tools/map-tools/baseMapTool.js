import { mapTool } from "..";
import { OlDefault } from "./OlDefault";

const baseMaps = [
    {
        id: 'NONE',
        source: null
    },
    {
        id: 'GR',
        source: mapTool.createXyzSource({
            crossOrigin: 'Anonymous',
            url: 'https://mts{0-3}.google.com/vt/lyrs=m&x={x}&y={y}&z={z}&apistyle=s.t:2%7Cs.e:l%7Cp.v:off,s.t:20%7Cs.e:l%7Cp.v:off'
        })
    },
    {
        id: 'GS',
        source: mapTool.createXyzSource({
            crossOrigin: 'Anonymous',
            url: 'https://mts{0-3}.google.com/vt/lyrs=y&x={x}&y={y}&z={z}'
        })
    },
    {
        id: 'GP',
        source: mapTool.createXyzSource({
            crossOrigin: 'Anonymous',
            url: 'https://mts{0-3}.google.com/vt/lyrs=p&x={x}&y={y}&z={z}'
        })
    },
    {
        id: 'BR',
        source: mapTool.createBingSource({
            crossOrigin: 'Anonymous',
            imagerySet: 'Road'
        })
    },
    {
        id: 'BS',
        source: mapTool.createBingSource({
            crossOrigin: 'Anonymous',
            imagerySet: 'Aerial'
        })
    },
    {
        id: 'BP',
        source: mapTool.createBingSource({
            crossOrigin: 'Anonymous',
            imagerySet: 'AerialWithLabels'
        })
    },
    {
        id: 'OR',
        source: mapTool.createOsmSource({
            crossOrigin: 'Anonymous',
            url: 'https://{a-c}.tile.openstreetmap.fr/hot/{z}/{x}/{y}.png'
        })
    },
    {
        id: 'OS',
        source: mapTool.createOsmSource({
            crossOrigin: 'Anonymous',
            url: 'https://{a-c}.tile.opencyclemap.org/cycle/{z}/{x}/{y}.png'
        })
    },
    {
        id: 'OP',
        source: mapTool.createOsmSource({
            crossOrigin: 'Anonymous',
            url: 'https://otile1-s.mqcdn.com/tiles/1.0.0/osm/{z}/{x}/{y}.png'
        })
    },
    {
        id: 'BM',
        source: mapTool.createXyzSource({
            crossOrigin: 'Anonymous',
            url: 'https://bms.basarsoft.com.tr/Service/api/v1/map/Default?accId=GL8VCF9E5k2AkIroN5fC6w&appCode=qEw539H1eUWYiRXO6LhB1w&x={x}&y={y}&z={z}'
        })
    },
];

const addBaseMap = (initialBaseMap = "BM") => {

    let baseLayer = mapTool.createTileLayer({
        name: OlDefault.olBaseLayer.layerTitle,
        source: baseMaps.find(x => x.id === initialBaseMap).source
    });

    mapTool.addLayer(baseLayer);
}

const changeBaseMap = (selectedBaseMap) => {

    let baseMapLayer = mapTool.findLayer(OlDefault.olBaseLayer.layerTitle);

    if (baseMapLayer) {

        const baseMap = baseMaps.find(x => x.id === selectedBaseMap);

        const source = baseMap ? baseMap.source : null;

        baseMapLayer.setSource(source);

        if (source != null) {

            let tileLoading = 0;

            let tileLoaded = 0;

            let run = 0;

            source.on('tileloadstart', x => ++tileLoading);

            source.on('tileloadend', x => {

                ++tileLoaded;

                if (tileLoaded === tileLoading && run <= 0) {

                    run++;
                }
            });
        }
    }
}

export const baseMapTool = {
    addBaseMap,
    changeBaseMap
}

export default baseMapTool;