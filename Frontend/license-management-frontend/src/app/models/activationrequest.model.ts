export class ActivationSettings {
    public hardwareId!: string;
    public email!: string;
    public company!: string;
    public location!: string;
    public customerCode!: string;
    public hashSettings!: string;
}

export class ActivationRequest {
    public id!: string;
    public activationCode!: string;
    public activationSettings!: ActivationSettings;
    public status!: string;
    public activationResult!: string;
}