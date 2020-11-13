export class LicenseSettings {
    public hardwareId!: string;
    public email!: string;
    public company!: string;
    public location!: string;
    public customerCode!: string;
    public hashSettings!: string;
}

export class License {
    public id!: string;
    public licenseHash!: string;
    public activationCode!: string;
    public activationDate!: Date;
    public lifeTime!: number;
    public type!: string;
    public licenseSettings: LicenseSettings = new LicenseSettings;
}