import constant from '../constants/layerConstant';
import { layerService } from '../services';
import { toast } from 'react-toastify';

const getAll = () => {

    return dispatch => {

        dispatch(request());

        layerService.getAll().
            then(response => {

                if (!response.success) {

                    toast.error(response.exception);

                    dispatch(fail(response.exception));

                    return;
                }

                dispatch(success(response.data));
            }).
            catch(ex => {

                toast.error("Katmanlar getirilemedi.");

                dispatch(fail("Katmanlar getirilemedi."));
            });
    };

    function request() { return { type: constant.GET_ALL_REQUEST }; }

    function success(value) { return { type: constant.GET_ALL_SUCCESS, payload: value }; }
    
    function fail(value) { return { type: constant.GET_ALL_FAIL, payload: value }; }
}

const getFeatureInfo = (coordinates) => {

    return dispatch => {

        dispatch(request());

        layerService.getFeatureInfo(coordinates).
            then(response => {

                if (!response.success) {

                    toast.error(response.exception);

                    dispatch(fail(response.exception));

                    return;
                }

                if(response.data.length === 0)
                {
                    toast.error("Sonuç bulunamadı.");

                    dispatch(fail("Sonuç bulunamadı."));

                    return;
                }

                const data = []

                for (const item of response.data) {

                    let label = "";

                    for (const property in item.properties) {

                        label += property + ": " + item.properties[property] + ", ";
                    }

                    data.push({
                        key: item.id,
                        id: item.idColumn + ": " + item.id,
                        layer: item.name,
                        label: label.substr(0, label.length - 2)
                    });
                }

                dispatch(success(data));
            }).
            catch(ex => {

                toast.error("Bilgi alınamadı.");

                dispatch(fail("Bilgi alınamadı."));
            });
    };

    function request() { return { type: constant.GET_FEATURE_INFO_REQUEST }; }

    function success(value) { return { type: constant.GET_FEATURE_INFO_SUCCESS, payload: value }; }

    function fail(value) { return { type: constant.GET_FEATURE_INFO_FAIL, payload: value }; }
}

export const layerAction = {
    getAll,
    getFeatureInfo
}

export default layerAction;