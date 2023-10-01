import { Component ,OnInit} from '@angular/core';
import { Router } from '@angular/router';
import { PedalProServiceService } from 'src/app/Services/pedal-pro-service.service';
import { TrainingModule } from 'src/app/Models/training-module';
import { MatDialog } from '@angular/material/dialog';
import { ErrorDialogComponent } from 'src/app/Dialogs/error-dialog/error-dialog.component';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-client-upload-if',
  templateUrl: './client-upload-if.component.html',
  styleUrls: ['./client-upload-if.component.css']
})
export class ClientUploadIFComponent implements OnInit{
  selectedFile: File | null = null;
  selectedFileTwo: File | null = null;
  title: string = '';
  clientDetails: any;
  cartnumber:any;
  modules:TrainingModule[]=[];
  documentUrl: SafeResourceUrl | undefined;
  documentContent: Blob | null = null;

  constructor(private dialog:MatDialog,private documentUploadService: PedalProServiceService, private router:Router,private sanitizer: DomSanitizer) {}

  ngOnInit(): void {
    this.GetModules();
    const storedCartQuantity = localStorage.getItem('cartQuantity');
    this.cartnumber = storedCartQuantity ? parseInt(storedCartQuantity, 10) : 0;
    this.fetchClientDetails();
    
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0] as File;
  }

  fetchClientDetails() {
    this.documentUploadService.getClientDetails().subscribe(
      (response) => {
        this.clientDetails = response;
      },
      (err)=>{
        const errorMessage = err.error || 'An error occurred';
        this.openErrorDialog(errorMessage);
      }
    );
  }

  openErrorDialog(errorMessage: string): void {
    this.dialog.open(ErrorDialogComponent, {
      data: { message: errorMessage }
    });
  }

  toggleHelp() {
    const modelDiv = document.getElementById('helphelp');
    if (modelDiv != null) {
      if (modelDiv.style.display === 'block') {
        modelDiv.style.display = 'none';
      } else {
        modelDiv.style.display = 'block';
      }
    }
  }

  onUpload() {
    if (!this.selectedFile || !this.title) {
      this.openErrorDialog('Please select a file and provide a title.');
      return;
    }

    this.documentUploadService.uploadDocumentClient(this.selectedFile, this.title).subscribe(
      (response) => {
        this.openModal();
      },
      (err)=>{
        const errorMessage = err.error || 'An error occurred';
        this.openErrorDialog(errorMessage);
      }
    );
  }

  Logout()
  {
    this.documentUploadService.logout();
  }

  // get modules method
  GetModules(){
    this.documentUploadService.GetModules().subscribe(result=>{
      let moduleList:any[]=result
      moduleList.forEach((element)=>{
        this. modules.push(element)
      });
    })
    return this.modules;
  }

  onFileSelectedtwo(event: any) {
    this.selectedFile = event.target.files[0];
  }

  onUploadtwo() {
    if (!this.selectedFile) {
      return;
    }

    const formData = new FormData();
    formData.append('file', this.selectedFile);

    this.documentUploadService.uploadImageIndemnity(formData).subscribe(
      (response) => {
        this.openModal();
      },
      (err)=>{
        const errorMessage = err.error || 'An error occurred';
        this.openErrorDialog(errorMessage);
      }
    );
  }

  cancel_continue() {
    this.router.navigate(['/clientLanding']);
  }

  // modal pop-up
  openModal() {
    const modelDiv = document.getElementById('myModal');
    if (modelDiv != null) {
      modelDiv.style.display = 'block';
    }

    this.getDocument();
  }

  getDocument(): void {
    this.documentUploadService.getLatestPdfDocument().subscribe(
      (documentBlob: Blob) => {
        this.documentContent = documentBlob;
        const blobUrl = URL.createObjectURL(documentBlob);
        this.documentUrl = this.sanitizer.bypassSecurityTrustUrl(blobUrl);

        this.downloadDocument();
      },
      (err)=>{
        const errorMessage = err.error || 'An error occurred';
        const errmess='No form available'
        this.openErrorDialog(errmess);
      }
    );
  }

  downloadDocument(): void {
    if (this.documentContent) {
      const downloadLink = document.createElement('a');
      const blobUrl = URL.createObjectURL(this.documentContent);
      downloadLink.href = blobUrl;
      downloadLink.download = 'document.pdf';
      downloadLink.click();
      URL.revokeObjectURL(blobUrl);
    }
  }
}
