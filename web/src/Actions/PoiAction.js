import constant from '../constants/poiConstant';
import { poiService } from '../services';
import { toast } from 'react-toastify';

const get = (item) => {

    return dispatch => {

        dispatch(request());

        poiService.get(item).
            then(response => {

                if (!response.success) {

                    toast.error(response.exception);

                    dispatch(fail(response.exception));

                    return;
                }

                dispatch(success(response.data));
            }).
            catch(ex => {

                toast.error("Poi getirilimedi.");

                dispatch(fail("Poi getirilimedi."));
            });
    };

    function request () { return { type: constant.GET_REQUEST }; }

    function success(value) { return { type: constant.GET_SUCCESS, payload: value }; }

    function fail(value) { return { type: constant.GET_FAIL, payload: value }; }
}

const add = (item) => {

    return dispatch => {

        dispatch(request());

        poiService.add(item).
            then(response => {

                if (!response.success) {

                    toast.error(response.exception);

                    dispatch(fail(response.exception));

                    return;
                }

                toast.success("Poi eklendi.");

                dispatch(success(response.data));
            }).
            catch(ex => {

                toast.error("Poi eklenemedi.");

                dispatch(fail("Poi eklenemedi."));
            });
    };

    function request() { return { type: constant.ADD_REQUEST }; }

    function success(value) { return { type: constant.ADD_SUCCESS, payload: value }; }
    
    function fail(value) { return { type: constant.ADD_FAIL, payload: value }; }
}


const update = (item) => {

    return dispatch => {

        dispatch(request());

        poiService.update(item).
            then(response => {

                if (!response.success) {

                    toast.error(response.exception);

                    dispatch(fail(response.exception));

                    return;
                }

                toast.success("Poi güncellendi.");

                dispatch(success(response.data ?? item));
            }).
            catch(ex => {

                toast.error("Poi güncellenemedi.");

                dispatch(fail("Poi güncellenemedi."));
            });
    };

    function request() { return { type: constant.UPDATE_REQUEST }; }

    function success(value) { return { type: constant.UPDATE_SUCCESS, payload: value }; }
    
    function fail(value) { return { type: constant.UPDATE_FAIL, payload: value }; }
}

const reset = () => { return { type: constant.RESET } }

export const poiAction = {
    get,
    add,
    update,
    reset
}

export default poiAction;
