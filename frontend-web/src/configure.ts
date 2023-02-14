import {OpenAPI as FunctionIntegrationYnab} from "./generated/App.Function.Integration.Ynab";

export function configure(getAccessToken: () => Promise<string|null>)
{
    FunctionIntegrationYnab.TOKEN = async () => (await getAccessToken()) || ""
    FunctionIntegrationYnab.BASE = process.env.REACT_APP_FUNCTION_INTEGRATION_YNAB_BASE || FunctionIntegrationYnab.BASE;
}