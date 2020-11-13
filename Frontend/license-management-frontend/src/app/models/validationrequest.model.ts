export class ValidationSettings {
    public hardwareId!: string;
    public email!: string;
    public company!: string;
    public location!: string;
    public customerCode!: string;
    public hashSettings!: string;
}

export class ValidationRequest {
    public id!: string;
    public licenseCode!: string;
    public validationSettings: ValidationSettings = new ValidationSettings;
    public status!: string;
    public validationResult!: string;
    public requestDate!: Date;
}