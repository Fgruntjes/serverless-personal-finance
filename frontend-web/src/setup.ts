import {OpenAPI as FunctionIntegrationYnab} from "./generated/App.Function.Integration.Ynab";

export function setup()
{
    FunctionIntegrationYnab.BASE = process.env.REACT_APP_FUNCTION_INTEGRATION_YNAB_BASE || FunctionIntegrationYnab.BASE;
}