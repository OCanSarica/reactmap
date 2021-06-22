import { requestTool, mapTool } from "../tools";

const getAll = () => requestTool.getRequest(window.MAP_API_URL + "/api/layer");

const getLegendSource = (layerId) => window.MAP_API_URL + "/api/proxy/getlegend/" + layerId;

const getFeatureInfo = (coordinates) => {

    const url = getFeatureInfoUrl({
        layerName: 'wms_layer',
        coordinates,
        pixelRadius: 5,
        featureCount: 10
    });

    return requestTool.getRequest(url);
}

const getFeatureInfoUrl = (options) => {

    const url = mapTool.getLayerSource(options.layerName).
        getFeatureInfoUrl(options.coordinates,
            mapTool.getResolution(),
            mapTool.getProjection(),
            {
                'INFO_FORMAT': 'application/json',
                'PIXELRADIUS': options.pixelRadius,
                'FEATURE_COUNT': options.featureCount
            });

    return url.replace("/getmap?", "/getfeatureinfo?");
}

export const layerService = { getAll, getLegendSource, getFeatureInfo };

export default layerService;

