import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import {
  VisNode,
  VisNodes,
  VisEdges,
  VisNetworkService,
  VisNetworkData,
  VisNetworkOptions } from 'ng2-vis/components/network';

class ExampleNetworkData implements VisNetworkData {
    public nodes: VisNodes;
    public edges: VisEdges;
}

@Component({
  selector: 'app-visnetwork',
  templateUrl: './visnetwork.component.html',
  styleUrls: ['./visnetwork.component.css']
})
export class VisnetworkComponent implements OnInit, OnDestroy {
  @ViewChild('fileImportInput')
  fileImportInput: any;
  public visNetwork: string = 'networkId1';
  public visNetworkData: ExampleNetworkData;
  public visNetworkOptions: VisNetworkOptions;

  public constructor(private visNetworkService: VisNetworkService) {  }

  public addNode(): void {
    const newId = this.visNetworkData.nodes.getLength() + 1;
    this.visNetworkData.nodes.add({ id: newId.toString(), label: 'Node ' + newId });
    this.visNetworkService.fit(this.visNetwork);
}

public networkInitialized(): void {
    // now we can use the service to register on events
    this.visNetworkService.on(this.visNetwork, 'click');

    // open your console/dev tools to see the click params
    this.visNetworkService.click
        .subscribe((eventData: any[]) => {
            if (eventData[0] === this.visNetwork) {
              console.log(eventData[1]);
            }
        });
}

// METHOD CALLED WHEN CSV FILE IS IMPORTED
fileChangeListener($event): void {
  var text = [];
  var target = $event.target || $event.srcElement;
  var files = target.files; 
  var input = $event.target;
  var reader = new FileReader();
  var self = this;

  reader.onload = (data) => {
    let dotData = {
      dot: reader.result
    };
    self.visNetworkService.setData(self.visNetwork, <any>dotData);
  }

  reader.onerror = function () {
    alert('Unable to read ' + input.files[0]);
  };

  reader.readAsText(input.files[0]);
};

  ngOnInit() {
    var file = new FileReader();

    const nodes = new VisNodes([
      { id: '1', label: 'Node 1' },
      { id: '2', label: 'Node 2' },
      { id: '3', label: 'Node 3' },
      { id: '4', label: 'Node 4' },
      { id: '5', label: 'Node 5', title: 'Title of Node 5' }]);

    const edges = new VisEdges([
        { from: '1', to: '3' },
        { from: '1', to: '2' },
        { from: '2', to: '4' },
        { from: '2', to: '5' }]);

    this.visNetworkData = {
        nodes,
        edges,
    };

    this.visNetworkOptions = {};
  }

  ngOnDestroy() {
    this.visNetworkService.off(this.visNetwork, 'click');
  }

}
