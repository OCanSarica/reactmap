import constant from '../constants/poiConstant';

const initialState = {
    poies: {
        data: null,
        error: null,
        isLoading: false
    },
    poi: {
        data: null,
        error: null,
        isLoading: false
    },
    addedPoi: {
        data: null,
        error: null,
        isLoading: false
    },
    updatedPoi: {
        data: null,
        error: null,
        isLoading: false
    },
}

export function poiReducer(state = initialState, action) {

    switch (action.type) {

        case constant.GET_ALL_REQUEST:
            return {
                ...state,
                poies: {
                    data: null,
                    error: null,
                    isLoading: true
                }
            };

        case constant.GET_ALL_SUCCESS:
            return {
                ...state,
                poies: {
                    data: action.payload,
                    error: null,
                    isLoading: false
                }
            };

        case constant.GET_ALL_FAIL:
            return {
                ...state,
                poies: {
                    data: null,
                    error: action.payload,
                    isLoading: false
                }
            };

        case constant.GET_REQUEST:
            return {
                ...state,
                poi: {
                    data: null,
                    error: null,
                    isLoading: true
                }
            };

        case constant.GET_SUCCESS:
            return {
                ...state,
                poi: {
                    data: action.payload,
                    error: null,
                    isLoading: false
                }
            };

        case constant.GET_FAIL:
            return {
                ...state,
                poi: {
                    data: null,
                    error: action.payload,
                    isLoading: false
                }
            };

        case constant.ADD_REQUEST:
            return {
                ...state,
                addedPoi: {
                    data: null,
                    error: null,
                    isLoading: true
                }
            };

        case constant.ADD_SUCCESS:
            return {
                ...state,
                addedPoi: {
                    data: action.payload,
                    error: null,
                    isLoading: false
                }
            };

        case constant.ADD_FAIL:
            return {
                ...state,
                addedPoi: {
                    data: null,
                    error: action.payload,
                    isLoading: false
                }
            };

        case constant.UPDATE_REQUEST:
            return {
                ...state,
                updatedPoi: {
                    data: null,
                    error: null,
                    isLoading: true
                }
            };

        case constant.UPDATE_SUCCESS:
            return {
                ...state,
                updatedPoi: {
                    data: action.payload,
                    error: null,
                    isLoading: false
                }
            };

        case constant.UPDATE_FAIL:
            return {
                ...state,
                updatedPoi: {
                    data: null,
                    error: action.payload,
                    isLoading: false
                }
            };

        case constant.RESET:
            return {
                ...state,
                poi: initialState.poi,
                addedPoi: initialState.addedPoi,
                updatedPoi: initialState.updatedPoi,
            };

        default:
            return state;
    }
}

export default poiReducer;
