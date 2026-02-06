export interface ApiResponse<T> {
    success: boolean;
    message: string;
    data: T;
}

export interface ProjectDto {
    id: number;
    name: string;
    clientName: string;
    projectValue: number;
    startDate: string;
    endDate?: string;
    status: string;
    managedByPartner: string;
}

export interface CreateProjectDto {
    name: string;
    description?: string;
    clientName: string;
    projectValue: number;
    startDate: string;
    endDate?: string;
    managedByPartnerId: number;
}