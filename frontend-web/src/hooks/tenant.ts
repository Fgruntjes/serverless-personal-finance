import {useState} from "react";

interface TenantContext {
    currentTenant: string|undefined,
}

export function useTenant(): TenantContext {
    // Tenants are not implemented
    const [currentTenant] = useState<string>("eaa60c18-d93e-4fc3-9f35-760c4879ddce");
    
    return {currentTenant};
}
