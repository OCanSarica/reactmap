import { requestTool } from "../tools";

const CONTROLLER_URL = "/api/poi";

const getAll = () => requestTool.getRequest(window.CRUD_API_URL + CONTROLLER_URL);

const get = (id) => requestTool.getRequest(window.CRUD_API_URL + CONTROLLER_URL + "/" + id);

const add = (item) => requestTool.postRequest(window.CRUD_API_URL + CONTROLLER_URL, item);

const update = (item) => requestTool.putRequest(window.CRUD_API_URL + CONTROLLER_URL, item);

const remove = (id) => requestTool.deleteRequest(window.CRUD_API_URL + CONTROLLER_URL + "/" + id);

const paginate = (options) => 
    requestTool.postRequest(window.CRUD_API_URL + CONTROLLER_URL + "/paginate", options);

const getGeometry = (id) => 
    requestTool.getRequest(window.CRUD_API_URL + CONTROLLER_URL + "/geometry/" + id);

export const poiService = { getAll, add, remove, update, get, paginate, getGeometry };

export default poiService;