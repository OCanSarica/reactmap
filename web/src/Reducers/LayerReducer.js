import constant from '../constants/layerConstant';

const initialState = {
    layers: {
        data: null,
        error: null,
        isLoading: false
    },
    infoResults: {
        data: null,
        error: null,
        isLoading: false
    }
}

export function layerReducer(state = initialState, action) {

    switch (action.type) {

        case constant.GET_ALL_REQUEST:
            return {
                ...state,
                layers: {
                    data: null,
                    error: null,
                    isLoading: true
                }
            };

        case constant.GET_ALL_SUCCESS:
            return {
                ...state,
                layers: {
                    data: action.payload,
                    error: null,
                    isLoading: false
                }
            };

        case constant.GET_ALL_FAIL:
            return {
                ...state,
                layers: {
                    data: null,
                    error: action.payload,
                    isLoading: false
                }
            };

        case constant.GET_FEATURE_INFO_REQUEST:
            return {
                ...state,
                infoResults: {
                    data: null,
                    error: null,
                    isLoading: true
                }
            };

        case constant.GET_FEATURE_INFO_SUCCESS:
            return {
                ...state,
                infoResults: {
                    data: action.payload,
                    error: null,
                    isLoading: false
                }
            };

        case constant.GET_FEATURE_INFO_FAIL:
            return {
                ...state,
                infoResults: {
                    data: null,
                    error: action.payload,
                    isLoading: false
                }
            };

        default:
            return state;
    }
}

export default layerReducer;
